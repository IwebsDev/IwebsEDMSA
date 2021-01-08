using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using System.Collections;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IReportService" à la fois dans le code et le fichier de configuration.
    
    [ServiceContract]
    public interface IReportService 
    {
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ReturneAnnulationFactureTop(List<int> lstIdCentre, DateTime DateDebut, DateTime DateFin, int Top);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneDevisTerminerDsLesDelais(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneDevisTerminerDsLesDelais_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneDevisTerminerHorsDelais(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneDevisTerminerHorsDelais_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande,List<string> produit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneTravauxRealiserDsDelais(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneTravauxRealiserDsDelais_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneTravauxRealiserHorsDelais(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneTravauxRealiserHorsDelais_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneTravauxRealiser(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneTravauxRealiser_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneDemandeParType(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsCanalisation> ReturnecompteursDisponiblesEnMagasin(List<int> lstCentre, int? idCalibreCompteur, string produit, string EtatCompteur, DateTime? debut, DateTime? fin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> ReturneListeDesImpayes(Dictionary<string, List<int>> lstCentre, List<int> lstIdCategorieClient, List<int> lstIdTournee, bool IsDetail);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneDevisPayeEnInstanceDeLiaison(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneDevisPayeEnInstanceDeLiaison_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande,List< string> produit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneDemandeEnAttenteDeRealisation(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneDemandeEnAttenteDeRealisation_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneRegistreDemande(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit);
        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDemandeBase> ReturneRegistreDemande_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRedevanceFacture> ReturneVentePeriodeAnnee(List<int> lstIdCentre, string periode, string Annee, bool IsRecap);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement > ReturneNombreMoyenDeFacturation(string CodeSite, string Periode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRedevanceFacture> ReturneVenteCummule(List<int> lstIdCentre, string Periode, bool IsSatistique);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ReturneConsNull(Dictionary<string, List<int>> lstSiteCentre, string Periode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ReturneActionFacturation(List<int> lstIdCentre, string Periode, string Lotri);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ReturneCompteurParProduit(List<int> lstIdCentre, List<int> lstIdProduit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ReturneCompteurParProduitPeriode(Dictionary<string, List<int>> lesDeCentre, string periode, bool IsStat);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ReturneFactureIsole(List<int> lstIdCentre, DateTime DateDebut, DateTime DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsEvenement> ReturneAnnulationFacture(List<int> lstIdCentre, DateTime DateDebut, DateTime DateFin);


        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ReturneAvisEmis(string CodeSite, List<int> IdMatricule, DateTime DateDebut, DateTime DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ReturneAvisCoupeType(string CodeSite, List<int> IdMatricule, DateTime DateDebut, DateTime DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ReturneMontant(int IdCentre, string Matricule, DateTime DateDebut, DateTime DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ReturneAvisRepose(string CodeSite, List<int> IdMatricule, DateTime DateDebut, DateTime DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> RetourneClientResilieSuiteCampagne(int IdCentre, string Matricule, DateTime DateDebut, DateTime DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneTauxDeRecouvrement(List<int> IdCentre);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> RetourneTauxDeEncaissement(List<int> IdCentre);



        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> MontantPaiementPreavis(string Matricule, DateTime DateDebut, DateTime DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsUtilisateur> RetourneGestionnaire();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ListePreavisPreavis(string Matricule, DateTime DateDebut, DateTime DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMandatementGc> ListeMandatement(string Matricule, DateTime DateDebut, DateTime DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsDetailCampagne> ListePaiementMandatement(string Matricule, DateTime DateDebut, DateTime DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMandatementGc> RetourneTauxDeMandatement(string Matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsMandatementGc> RetourneTauxPaiement(string Matricule);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> ReturneEmissionProduitRegroupement(List<int> IdRegroupement, string PeriodeDebut, string periodefin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> ReturneAvanceSurConso(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> ReturneEncaissementReversement(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> ReturneEncaissementModePaiement(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsStatFactRecap> ReturneVente(List<int> Idcentre, string PeriodeDebut, string periodefin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> ReturneClientPrepayeSansAchatPeriode(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsLclient> ReturneClientPrepayeJamaisAchat(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsComptabilisation> ReturneCompabilisationRecap(List<int> Idcentre, string periode, bool IsGroup);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsStatFact> ReturneStatistique(string CodeSite, string periode, string Produit, bool IsStat);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsRedevance> ChargerRedevance();

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsComptabilisation> ReturneStatiqueDesVente(List<int> Idcentre, string periode, bool IsGrouper);

        //[OperationContract]
        //[FaultContract(typeof(Errors))]
        //List<CsStatFact> ReturneStatiqueDesVenteStat(List<int> Idcentre, string periode, string Produit);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        void ReturneFichierPersonnel(string periode, string CheminImpression);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTranscaisse> ReturneEncaissementParMoisComptable(List<int> lesDeCentre, string periode);

        [OperationContract]
        [FaultContract(typeof(Errors))]
        List<CsTournee> ReturneTourneePIA(string CodeSite);
    }
}
