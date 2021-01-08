using Galatee.Entity.Model.Rpnt;
using Galatee.Structure;
using Galatee.Structure.Rpnt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Entity.Model.Rpnt
{
    public class RpntProcedures
    {
        
        //public static DataTable VuevwNbreElementsNonSaisisParLotDeControleHTAEditeNonTermine()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {

        //            IEnumerable<object> query =
        //    from ElementLot in context.TBELEMENTLOTDECONTROLEHTA
        //    group new { ElementLot.TBLOTDECONTROLEHTA, ElementLot } by new
        //    {
        //        CAMPAGNE_ID = (Guid?)ElementLot.TBLOTDECONTROLEHTA.CAMPAGNE_ID,
        //        LOT_ID = (Guid?)ElementLot.TBLOTDECONTROLEHTA.LOT_ID,
        //        STATUTLOT_ID = (int?)ElementLot.TBLOTDECONTROLEHTA.STATUTLOT_ID
        //    } into g
        //    where g.Key.STATUTLOT_ID < 4
        //    select new
        //    {
        //        CAMPAGNE_ID = (Guid?)g.Key.CAMPAGNE_ID,
        //        LOT_ID = (Guid?)g.Key.LOT_ID,
        //        NbreElementsNonControles = g.Sum(p => (
        //        p.ElementLot.RESULTATCONTROLE_ID == null ? 1 : 0)),
        //        STATUTLOT_ID = (int?)g.Key.STATUTLOT_ID
        //    };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwNomsMachineAudites()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {

        //            IEnumerable<object> query =
        //    (from AUDITs in context.AUDIT
        //     select new
        //     {
        //         NomMachineValue = AUDITs.NOMMACHINE,
        //         NomMachineLibelle = AUDITs.NOMMACHINE
        //     }).Distinct();
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwParametrageSynchroAutomatiqueCodeSite()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {

        //            IEnumerable<object> query =
        //            from TBPARAMETRAGESYNCHROAUTOMATIQUEs in context.TBPARAMETRAGESYNCHROAUTOMATIQUE
        //            join REFEXPLOITATIONs in context.REFEXPLOITATION on TBPARAMETRAGESYNCHROAUTOMATIQUEs.CODEEXPLOITATION equals REFEXPLOITATIONs.CODEEXPLOITATION
        //            select new
        //            {
        //                TBPARAMETRAGESYNCHROAUTOMATIQUEs.CODEEXPLOITATION,
        //                TBPARAMETRAGESYNCHROAUTOMATIQUEs.DATEDEBUTVALIDITE,
        //                TBPARAMETRAGESYNCHROAUTOMATIQUEs.ESTACTIF,
        //                TBPARAMETRAGESYNCHROAUTOMATIQUEs.PERIODICITE,
        //                TBPARAMETRAGESYNCHROAUTOMATIQUEs.PARTIEHEURELANCEMENT_HEURE,
        //                TBPARAMETRAGESYNCHROAUTOMATIQUEs.PARTIEHEURELANCEMENT_MINUTE,
        //                TBPARAMETRAGESYNCHROAUTOMATIQUEs.DATEPROCHAINEEXECUTION,
        //                REFEXPLOITATIONs.EXPLOTATION_LIBELLE,
        //                REFEXPLOITATIONs.SITEGESABEL
        //            };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwParametrageSynchroAutomatiqueDelUO()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {

        //            IEnumerable<object> query =
        //            from TBPARAMETRAGESYNCHROAUTOMATIQUEs in context.TBPARAMETRAGESYNCHROAUTOMATIQUE
        //            join REFEXPLOITATION in context.REFEXPLOITATION on new { CODEEXPLOITATION = (string)TBPARAMETRAGESYNCHROAUTOMATIQUEs.CODEEXPLOITATION } equals new { CODEEXPLOITATION = REFEXPLOITATION.CODEEXPLOITATION }
        //            from REFUNITEORGANISATIONNELLE in REFEXPLOITATION.REFUNITEORGANISATIONNELLE
        //            select new
        //            {
        //                REFEXPLOITATION.CODEEXPLOITATION,
        //                REFUNITEORGANISATIONNELLE.CODEUO,
        //                TBPARAMETRAGESYNCHROAUTOMATIQUEs.DATEDEBUTVALIDITE,
        //                TBPARAMETRAGESYNCHROAUTOMATIQUEs.ESTACTIF,
        //                TBPARAMETRAGESYNCHROAUTOMATIQUEs.PERIODICITE,
        //                TBPARAMETRAGESYNCHROAUTOMATIQUEs.PARTIEHEURELANCEMENT_HEURE,
        //                TBPARAMETRAGESYNCHROAUTOMATIQUEs.PARTIEHEURELANCEMENT_MINUTE,
        //                TBPARAMETRAGESYNCHROAUTOMATIQUEs.DATEPROCHAINEEXECUTION
        //            };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwPeriodeFacturationLaPlusRecentePourLesBET()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            IEnumerable<object> query =
        //                from TBBETs in context.TBBET
        //                join AnMax in
        //                    (
        //                        (from tbBET_1 in context.TBBET
        //                         group tbBET_1 by new
        //                         {
        //                             tbBET_1.CODEEXPLOITATION
        //                         } into g
        //                         select new
        //                         {
        //                             g.Key.CODEEXPLOITATION,
        //                             AnneeMAX = (int?)g.Max(p => p.PERIODEFACT_AN)
        //                         }))
        //                      on new { AnneeMAX = TBBETs.PERIODEFACT_AN, TBBETs.CODEEXPLOITATION }
        //                  equals new { AnMax.AnneeMAX, AnMax.CODEEXPLOITATION }
        //                group TBBETs by new
        //                {
        //                    TBBETs.PERIODEFACT_AN,
        //                    TBBETs.CODEEXPLOITATION
        //                } into g
        //                select new
        //                {
        //                    g.Key.CODEEXPLOITATION,
        //                    AnneeDernierePeriode = (int?)g.Key.PERIODEFACT_AN,
        //                    MoisDernierePeriode = (int?)g.Max(p => p.PERIODEFACT_MOIS)
        //                };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwRechercheBranchementBTA()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            var AbonneBTA = from TBCONTRATBTAs in context.TBCONTRATBTA
        //                            join TBTIERSCLIENTs in context.TBTIERSCLIENT
        //                                  on new { TBCONTRATBTAs.CODEEXPLOITATION, TBCONTRATBTAs.REFERENCECLIENT }
        //                              equals new { TBTIERSCLIENTs.CODEEXPLOITATION, TBTIERSCLIENTs.REFERENCECLIENT }
        //                            select new
        //                            {
        //                                TBTIERSCLIENTs.CODEEXPLOITATION,
        //                                TBTIERSCLIENTs.REFERENCECLIENT,
        //                                TBTIERSCLIENTs.TYPECLIENT,
        //                                TBTIERSCLIENTs.TYPABON_ID,
        //                                TBTIERSCLIENTs.NOM,
        //                                TBTIERSCLIENTs.PRENOMS,
        //                                TBCONTRATBTAs.CONTRAT_ID,
        //                                TBCONTRATBTAs.BRANCHEMENT_ID,
        //                                TBCONTRATBTAs.RESULTATDERNIERCONTROLE_ID,
        //                                TBCONTRATBTAs.STATUTCONTRAT,
        //                                TBCONTRATBTAs.GROUPEDEFACTURATION,
        //                                TBCONTRATBTAs.PUISSANCESOUSCRITE,
        //                                TBCONTRATBTAs.TYPECONTRATID,
        //                                TBCONTRATBTAs.TYPETARIF_ID,
        //                                TBCONTRATBTAs.USAGEABONNEMENT_ID,
        //                                TBCONTRATBTAs.DATEABONNEMENT,
        //                                TBCONTRATBTAs.DATESTATUTCONTRAT,
        //                                TBCONTRATBTAs.DATEPROCHAINCONTROLEPOSTFRAUDE,
        //                                TBCONTRATBTAs.CONSTATDUCONTROLEPOSTFRAUDE_ID
        //                            };
        //            IEnumerable<object> query =
        //               from TBBRANCHEMENTs in context.TBBRANCHEMENT
        //               join AbonneBTAs in AbonneBTA on new { CONTRATCOURANT_ID = (Guid)TBBRANCHEMENTs.CONTRATCOURANT_ID } equals new { CONTRATCOURANT_ID = AbonneBTAs.CONTRAT_ID }
        //               select new
        //               {
        //                   TBBRANCHEMENTs.CODEEXPLOITATION,
        //                   TBBRANCHEMENTs.TYPEBRANCHEMENT_ID,
        //                   TBBRANCHEMENTs.BRANCHEMENT_ID,
        //                   Contrat_ID = (Guid?)AbonneBTAs.CONTRAT_ID,
        //                   AbonneBTAs.STATUTCONTRAT,
        //                   AbonneBTAs.REFERENCECLIENT,
        //                   AbonneBTAs.NOM,
        //                   AbonneBTAs.PRENOMS,
        //                   TBBRANCHEMENTs.STATUT_BRANCHEMENT
        //               };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwRechercheDesClientsBTAAControler()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {

        //            IEnumerable<object> query =
        //               from TBCONTRATBTAs in context.TBCONTRATBTA
        //               join TBTIERSCLIENTs in context.TBTIERSCLIENT on TBCONTRATBTAs.REFERENCECLIENT equals TBTIERSCLIENTs.REFERENCECLIENT
        //               join TBBRANCHEMENTs in context.TBBRANCHEMENT on TBCONTRATBTAs.BRANCHEMENT_ID equals TBBRANCHEMENTs.BRANCHEMENT_ID
        //               join TBTOURNEEs in context.TBTOURNEE on new { TOURNEE_ID = (Guid)TBBRANCHEMENTs.TOURNEE_ID } equals new { TOURNEE_ID = TBTOURNEEs.TOURNEE_ID }
        //               join TBRESULTATCONTROLEBTAs in context.TBRESULTATCONTROLEBTA on new { RESULTATDERNIERCONTROLE_ID = (Guid)TBCONTRATBTAs.RESULTATDERNIERCONTROLE_ID } equals new { RESULTATDERNIERCONTROLE_ID = TBRESULTATCONTROLEBTAs.RESULTATCONTROLE_ID } into TBRESULTATCONTROLEBTAs_join
        //               from TBRESULTATCONTROLEBTAs in TBRESULTATCONTROLEBTAs_join.DefaultIfEmpty()
        //               join TBCOMPTEURBTAs in context.TBCOMPTEURBTA
        //                     on new { TBBRANCHEMENTs.NUMERO_COMPTEUR, TBBRANCHEMENTs.CODEEXPLOITATION }
        //                 equals new { TBCOMPTEURBTAs.NUMERO_COMPTEUR, TBCOMPTEURBTAs.CODEEXPLOITATION } into TBCOMPTEURBTAs_join
        //               from TBCOMPTEURBTAs in TBCOMPTEURBTAs_join.DefaultIfEmpty()
        //               select new
        //               {
        //                   TBCONTRATBTAs.CONTRAT_ID,
        //                   TBTIERSCLIENTs.REFERENCECLIENT,
        //                   TBBRANCHEMENTs.BRANCHEMENT_ID,
        //                   TBCONTRATBTAs.CODEEXPLOITATION,
        //                   TBCONTRATBTAs.TYPECONTRATID,
        //                   TBTIERSCLIENTs.NOM,
        //                   TBTIERSCLIENTs.PRENOMS,
        //                   TBTIERSCLIENTs.TYPECLIENT,
        //                   TBCONTRATBTAs.GROUPEDEFACTURATION,
        //                   TYPECOMPTEUR_ID = (int?)TBCOMPTEURBTAs.TYPECOMPTEUR_ID,
        //                   LELIBELLETOURNEE = (TBTOURNEEs.LIBELLETOURNEE ?? ""),
        //                   TBBRANCHEMENTs.NUMERO_COMPTEUR,
        //                   TBCONTRATBTAs.PUISSANCESOUSCRITE,
        //                   TBCONTRATBTAs.TYPETARIF_ID,
        //                   TBCONTRATBTAs.STATUTCONTRAT,
        //                   TBCONTRATBTAs.USAGEABONNEMENT_ID,
        //                   RESULTATDERNIERCONTROLE_ID = (Guid?)TBCONTRATBTAs.RESULTATDERNIERCONTROLE_ID,
        //                   RESULTATVALUE = (int?)TBRESULTATCONTROLEBTAs.RESULTATVALUE,
        //                   MATRICULEAGENTCONTROLE = TBRESULTATCONTROLEBTAs.MATRICULEAGENTCONTROLE,
        //                   DATECONTROLE = (DateTime?)TBRESULTATCONTROLEBTAs.DATECONTROLE,
        //                   TBTOURNEEs.MATRICULEAZ,
        //                   TBCONTRATBTAs.DATEDERNIERESELECTIONENCAMPAGNE
        //               };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwRechercheDesClientsHTAAControler()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            IEnumerable<object> query =
        //               from Contrat in context.TBCONTRATHTA
        //               join Tiers in context.TBTIERSCLIENT on Contrat.REFERENCECLIENT equals Tiers.REFERENCECLIENT
        //               join Raccordement in context.TBRACCORDEMENT on Contrat.RACCORDEMENT_ID equals Raccordement.RACCORDEMENT_ID
        //               select new
        //               {
        //                   Tiers.REFERENCECLIENT,
        //                   Tiers.TYPECLIENT,
        //                   Tiers.NOM,
        //                   Tiers.PRENOMS,
        //                   Contrat.CONTRAT_ID,
        //                   Contrat.CODEEXPLOITATION,
        //                   Contrat.STATUTCONTRAT,
        //                   Raccordement.NUMERO_COMPTEUR,
        //                   Raccordement.RACCORDEMENT_ID,
        //                   Contrat.PUISSANCESOUSCRITE,
        //                   Contrat.TYPETARIF_ID,
        //                   Raccordement.TYPECOMPTAGEHTA,
        //                   Raccordement.ID_ENSEMBLETECHNIQUE,
        //                   Contrat.RESULTATDERNIERCONTROLE_ID,
        //                   RESULTATVALUE = (int?)Contrat.TBRESULTATCONTROLEHTA.RESULTATVALUE,
        //                   Contrat.TBRESULTATCONTROLEHTA.MATRICULEAGENTCONTROLE,
        //                   DATECONTROLE = (DateTime?)Contrat.TBRESULTATCONTROLEHTA.DATECONTROLE,
        //                   Contrat.DATEDERNIERESELECTIONENCAMPAGNE
        //               };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwRechercheRaccordementHTA()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            var AbonneHTA = from TBCONTRATHTAs in context.TBCONTRATHTA
        //                            join TBTIERSCLIENTs in context.TBTIERSCLIENT
        //                                  on new { TBCONTRATHTAs.CODEEXPLOITATION, TBCONTRATHTAs.REFERENCECLIENT }
        //                              equals new { TBTIERSCLIENTs.CODEEXPLOITATION, TBTIERSCLIENTs.REFERENCECLIENT }
        //                            select new
        //                            {
        //                                TBCONTRATHTAs.CODEEXPLOITATION,
        //                                TBCONTRATHTAs.REFERENCECLIENT,
        //                                TBTIERSCLIENTs.NOM,
        //                                TBTIERSCLIENTs.PRENOMS,
        //                                TBTIERSCLIENTs.TYPECLIENT,
        //                                TBTIERSCLIENTs.TYPABON_ID,
        //                                TBCONTRATHTAs.RACCORDEMENT_ID,
        //                                TBCONTRATHTAs.STATUTCONTRAT,
        //                                TBCONTRATHTAs.PUISSANCESOUSCRITE,
        //                                TBCONTRATHTAs.TYPETARIF_ID,
        //                                TBCONTRATHTAs.RAPPORTTC_INTENTREE,
        //                                TBCONTRATHTAs.RAPPORTTC_INTSORTIE,
        //                                TBCONTRATHTAs.NIVEAUTENSION_ID,
        //                                TBCONTRATHTAs.CONTRAT_ID,
        //                                TBCONTRATHTAs.RESULTATDERNIERCONTROLE_ID,
        //                                TBCONTRATHTAs.DATEPROCHAINCONTROLEPOSTFRAUDE,
        //                                TBCONTRATHTAs.CONSTATDUCONTROLEPOSTFRAUDE_ID
        //                            };
        //            IEnumerable<object> query =
        //               from TBRACCORDEMENTs in context.TBRACCORDEMENT
        //               join AbonneHTAs in AbonneHTA on new { CONTRATHTACOURANT_ID = (Guid)TBRACCORDEMENTs.CONTRATHTACOURANT_ID } equals new { CONTRATHTACOURANT_ID = AbonneHTAs.CONTRAT_ID }
        //               select new
        //               {
        //                   TBRACCORDEMENTs.CODEEXPLOITATION,
        //                   TBRACCORDEMENTs.RACCORDEMENT_ID,
        //                   AbonneHTAs.REFERENCECLIENT,
        //                   AbonneHTAs.NOM,
        //                   AbonneHTAs.PRENOMS,
        //                   Contrat_ID = (Guid?)AbonneHTAs.CONTRAT_ID
        //               };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwStatistiqueAnomaliesParSiege()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            var AnomaliesDetecteesBTA = from TypeAnomalie in context.REFTYPEANOMALIEBTA
        //                                        from LienResultatControle in TypeAnomalie.TBRESULTATCONTROLEBTA
        //                                        select new
        //                                        {
        //                                            LienResultatControle.RESULTATCONTROLE_ID,
        //                                            TypeAnomalie.TYPEANOMALIE_ID,
        //                                            TYPEANOMALIE_LIBELLE = TypeAnomalie.LIBELLE,
        //                                            FAMILLEANOMALIE_ID = (int?)TypeAnomalie.REFFAMILLEANOMALIEBTA.FAMILLEANOMALIE_ID,
        //                                            FAMILLEANOMALIE_LIBELLE = TypeAnomalie.REFFAMILLEANOMALIEBTA.LIBELLE,
        //                                            TypeAnomalie.SIEGEANOMALIE_ID
        //                                        };
        //            IEnumerable<object> query =
        //              from AnomaliesDetecteesBTAs in AnomaliesDetecteesBTA
        //              join REFSIEGEANOMALIEs in context.REFSIEGEANOMALIE on new { SiegeAnomalie_ID = (int)AnomaliesDetecteesBTAs.SIEGEANOMALIE_ID } equals new { SiegeAnomalie_ID = REFSIEGEANOMALIEs.SIEGEANOMALIE_ID }
        //              join Resultat in context.TBRESULTATCONTROLEBTA on new { ResultatControle_ID = AnomaliesDetecteesBTAs.RESULTATCONTROLE_ID } equals new { ResultatControle_ID = Resultat.RESULTATCONTROLE_ID }
        //              join REFEXPLOITATION in context.REFEXPLOITATION on new { CODEEXPLOITATION = (string)Resultat.CODEEXPLOITATION } equals new { CODEEXPLOITATION = REFEXPLOITATION.CODEEXPLOITATION }
        //              from REFUNITEORGANISATIONNELLE in REFEXPLOITATION.REFUNITEORGANISATIONNELLE
        //              select new
        //              {
        //                  ResultatControle_ID = (Guid?)AnomaliesDetecteesBTAs.RESULTATCONTROLE_ID,
        //                  AnomaliesDetecteesBTAs.TYPEANOMALIE_ID,
        //                  AnomaliesDetecteesBTAs.TYPEANOMALIE_LIBELLE,
        //                  AnomaliesDetecteesBTAs.FAMILLEANOMALIE_ID,
        //                  AnomaliesDetecteesBTAs.FAMILLEANOMALIE_LIBELLE,
        //                  SIEGEANOMALIE_ID = (int?)REFSIEGEANOMALIEs.SIEGEANOMALIE_ID,
        //                  REFSIEGEANOMALIEs.SIEGEANOMALIE_LIBELLE,
        //                  Resultat.DATECONTROLE,
        //                  Resultat.CODEEXPLOITATION,
        //                  REFUNITEORGANISATIONNELLE.CODEUO
        //              };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwStatistiquesCampagneBTA()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            var StatistiquesElementsLotDeControleDeCampagneBTA = from TBELEMENTLOTDECONTROLEBTAs in context.TBELEMENTLOTDECONTROLEBTA
        //                                                                 join TBLOTDECONTROLEBTAs in context.TBLOTDECONTROLEBTA on TBELEMENTLOTDECONTROLEBTAs.LOT_ID equals TBLOTDECONTROLEBTAs.LOT_ID
        //                                                                 join Resultat in context.TBRESULTATCONTROLEBTA on new { RESULTATCONTROLE_ID = (Guid)TBELEMENTLOTDECONTROLEBTAs.RESULTATCONTROLE_ID } equals new { RESULTATCONTROLE_ID = Resultat.RESULTATCONTROLE_ID } into Resultat_join
        //                                                                 from Resultat in Resultat_join.DefaultIfEmpty()
        //                                                                 group new { TBLOTDECONTROLEBTAs, Resultat } by new
        //                                                                 {
        //                                                                     TBLOTDECONTROLEBTAs.CAMPAGNE_ID
        //                                                                 } into g
        //                                                                 select new
        //                                                                 {
        //                                                                     CAMPAGNE_ID = (Guid?)g.Key.CAMPAGNE_ID,
        //                                                                     NbreElementsDansDesLotsDeControle = g.Count(p => p.TBLOTDECONTROLEBTAs.CAMPAGNE_ID != null),
        //                                                                     NbreElementsNonControles = g.Sum(p => (
        //                                                                     p.Resultat.RESULTATCONTROLE_ID == null ? 1 : 0)),
        //                                                                     NbreElementsControlesRAS = g.Sum(p => (
        //                                                                     p.Resultat.RESULTATCONTROLE_ID != null &&
        //                                                                     (Int64)((Int32?)p.Resultat.RESULTATVALUE ?? (Int32?)-1) == 1 ? 1 : 0)),
        //                                                                     NbreElementsControlesKO = g.Sum(p => (
        //                                                                     p.Resultat.RESULTATCONTROLE_ID != null &&
        //                                                                     (Int64)((Int32?)p.Resultat.RESULTATVALUE ?? (Int32?)-1) == 2 ? 1 : 0))
        //                                                                 };
        //            IEnumerable<object> query =
        //              from TBCAMPAGNECONTROLEBTAs in context.TBCAMPAGNECONTROLEBTA
        //              join StatistiquesElementsLotDeControleDeCampagneBTAs in StatistiquesElementsLotDeControleDeCampagneBTA on new { CAMPAGNE_ID = TBCAMPAGNECONTROLEBTAs.CAMPAGNE_ID } equals new { CAMPAGNE_ID = (Guid)StatistiquesElementsLotDeControleDeCampagneBTAs.CAMPAGNE_ID }
        //              join REFEXPLOITATION in context.REFEXPLOITATION on new { CODEEXPLOITATION = (string)TBCAMPAGNECONTROLEBTAs.CODEEXPLOITATION } equals new { CODEEXPLOITATION = REFEXPLOITATION.CODEEXPLOITATION }
        //              from REFUNITEORGANISATIONNELLE in REFEXPLOITATION.REFUNITEORGANISATIONNELLE
        //              select new
        //              {
        //                  CAMPAGNE_ID = (Guid?)TBCAMPAGNECONTROLEBTAs.CAMPAGNE_ID,
        //                  TBCAMPAGNECONTROLEBTAs.LIBELLE_CAMPAGNE,
        //                  REFUNITEORGANISATIONNELLE.CODEUO,
        //                  TBCAMPAGNECONTROLEBTAs.CODEEXPLOITATION,
        //                  TBCAMPAGNECONTROLEBTAs.STATUT_ID,
        //                  TBCAMPAGNECONTROLEBTAs.DATECREATION,
        //                  TBCAMPAGNECONTROLEBTAs.NBREELEMENTS,
        //                  TBCAMPAGNECONTROLEBTAs.DATEDEBUTCONTROLES,
        //                  TBCAMPAGNECONTROLEBTAs.DATEFINPREVUE,
        //                  NbreElementsControles_RAS = ((Int32?)StatistiquesElementsLotDeControleDeCampagneBTAs.NbreElementsControlesRAS ?? (Int32?)0),
        //                  NbreElementsControles_KO = ((Int32?)StatistiquesElementsLotDeControleDeCampagneBTAs.NbreElementsControlesKO ?? (Int32?)0)
        //              };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwStatistiquesCampagneHTA()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            var StatistiquesElementsLotDeControleDeCampagneHTA = from TBELEMENTLOTDECONTROLEHTAs in context.TBELEMENTLOTDECONTROLEHTA
        //                                                                 join TBLOTDECONTROLEHTAs in context.TBLOTDECONTROLEHTA on TBELEMENTLOTDECONTROLEHTAs.LOT_ID equals TBLOTDECONTROLEHTAs.LOT_ID
        //                                                                 join Resultat in context.TBRESULTATCONTROLEHTA on new { RESULTATCONTROLE_ID = (Guid)TBELEMENTLOTDECONTROLEHTAs.RESULTATCONTROLE_ID } equals new { RESULTATCONTROLE_ID = Resultat.RESULTATCONTROLE_ID } into Resultat_join
        //                                                                 from Resultat in Resultat_join.DefaultIfEmpty()
        //                                                                 group new { TBLOTDECONTROLEHTAs, Resultat } by new
        //                                                                 {
        //                                                                     TBLOTDECONTROLEHTAs.CAMPAGNE_ID
        //                                                                 } into g
        //                                                                 select new
        //                                                                 {
        //                                                                     CAMPAGNE_ID = (Guid?)g.Key.CAMPAGNE_ID,
        //                                                                     NbreElementsDansDesLotsDeControle = g.Count(p => p.TBLOTDECONTROLEHTAs.CAMPAGNE_ID != null),
        //                                                                     NbreElementsNonControles = g.Sum(p => (
        //                                                                     p.Resultat.RESULTATCONTROLE_ID == null ? 1 : 0)),
        //                                                                     NbreElementsControlesRAS = g.Sum(p => (
        //                                                                     p.Resultat.RESULTATCONTROLE_ID != null &&
        //                                                                     (Int64)((Int32?)p.Resultat.RESULTATVALUE ?? (Int32?)-1) == 1 ? 1 : 0)),
        //                                                                     NbreElementsControlesKO = g.Sum(p => (
        //                                                                     p.Resultat.RESULTATCONTROLE_ID != null &&
        //                                                                     (Int64)((Int32?)p.Resultat.RESULTATVALUE ?? (Int32?)-1) == 2 ? 1 : 0))
        //                                                                 };
        //            IEnumerable<object> query =
        //              from TBCAMPAGNECONTROLEHTAs in context.TBCAMPAGNECONTROLEHTA
        //              join StatistiquesElementsLotDeControleDeCampagneHTAs in StatistiquesElementsLotDeControleDeCampagneHTA on new { CAMPAGNE_ID = TBCAMPAGNECONTROLEHTAs.CAMPAGNE_ID } equals new { CAMPAGNE_ID = (Guid)StatistiquesElementsLotDeControleDeCampagneHTAs.CAMPAGNE_ID }
        //              join REFEXPLOITATION in context.REFEXPLOITATION on new { CODEEXPLOITATION = (string)TBCAMPAGNECONTROLEHTAs.CODEEXPLOITATION } equals new { CODEEXPLOITATION = REFEXPLOITATION.CODEEXPLOITATION }
        //              from REFUNITEORGANISATIONNELLE in REFEXPLOITATION.REFUNITEORGANISATIONNELLE
        //              select new
        //              {
        //                  CAMPAGNE_ID = (Guid?)TBCAMPAGNECONTROLEHTAs.CAMPAGNE_ID,
        //                  TBCAMPAGNECONTROLEHTAs.LIBELLE_CAMPAGNE,
        //                  TBCAMPAGNECONTROLEHTAs.CODEEXPLOITATION,
        //                  TBCAMPAGNECONTROLEHTAs.STATUT_ID,
        //                  TBCAMPAGNECONTROLEHTAs.DATECREATION,
        //                  TBCAMPAGNECONTROLEHTAs.NBREELEMENTS,
        //                  TBCAMPAGNECONTROLEHTAs.DATEDEBUTCONTROLES,
        //                  TBCAMPAGNECONTROLEHTAs.DATEFINPREVUE,
        //                  REFUNITEORGANISATIONNELLE.CODEUO,
        //                  NbreElementsControles_RAS = ((Int32?)StatistiquesElementsLotDeControleDeCampagneHTAs.NbreElementsControlesRAS ?? (Int32?)0),
        //                  NbreElementsControles_KO = ((Int32?)StatistiquesElementsLotDeControleDeCampagneHTAs.NbreElementsControlesKO ?? (Int32?)0)
        //              };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwStatistiquesDesBETNonTraites()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            var StatistiquesBET = from TBBETs in context.TBBET
        //                                  join REFEXPLOITATION in context.REFEXPLOITATION on new { CODEEXPLOITATION = (string)TBBETs.CODEEXPLOITATION } equals new { CODEEXPLOITATION = REFEXPLOITATION.CODEEXPLOITATION }
        //                                  join ADMAGENTS in context.ADMAGENTS on new { MATRICULEAGENTZONE = TBBETs.MATRICULEAGENTZONE } equals new { MATRICULEAGENTZONE = ADMAGENTS.MATRICULEAGENT }
        //                                  join LibelleCodeano_1 in context.REFANOMALIESFACTURATION on new { CODANO_1 = TBBETs.CODANO_1 } equals new { CODANO_1 = LibelleCodeano_1.CODEANOMALIE } into LibelleCodeano_1_join
        //                                  from LibelleCodeano_1 in LibelleCodeano_1_join.DefaultIfEmpty()
        //                                  join LibelleCodeano_2 in context.REFANOMALIESFACTURATION on new { CODANO_2 = TBBETs.CODANO_2 } equals new { CODANO_2 = LibelleCodeano_2.CODEANOMALIE } into LibelleCodeano_2_join
        //                                  from LibelleCodeano_2 in LibelleCodeano_2_join.DefaultIfEmpty()
        //                                  join LibelleCodeano_3 in context.REFANOMALIESFACTURATION on new { CODANO_3 = TBBETs.CODANO_3 } equals new { CODANO_3 = LibelleCodeano_3.CODEANOMALIE } into LibelleCodeano_3_join
        //                                  from LibelleCodeano_3 in LibelleCodeano_3_join.DefaultIfEmpty()
        //                                  join LibelleCodeano_4 in context.REFANOMALIESFACTURATION on new { CODANO_4 = TBBETs.CODANO_4 } equals new { CODANO_4 = LibelleCodeano_4.CODEANOMALIE } into LibelleCodeano_4_join
        //                                  from LibelleCodeano_4 in LibelleCodeano_4_join.DefaultIfEmpty()
        //                                  join LibelleCodeano_5 in context.REFANOMALIESFACTURATION on new { CODANO_5 = TBBETs.CODANO_5 } equals new { CODANO_5 = LibelleCodeano_5.CODEANOMALIE } into LibelleCodeano_5_join
        //                                  from LibelleCodeano_5 in LibelleCodeano_5_join.DefaultIfEmpty()
        //                                  join AdmAgentModification in context.ADMAGENTS on new { MATRICULEAGENTMODIFICATION = TBBETs.MATRICULEAGENTMODIFICATION } equals new { MATRICULEAGENTMODIFICATION = AdmAgentModification.MATRICULEAGENT } into AdmAgentModification_join
        //                                  from AdmAgentModification in AdmAgentModification_join.DefaultIfEmpty()
        //                                  from REFUNITEORGANISATIONNELLE in REFEXPLOITATION.REFUNITEORGANISATIONNELLE
        //                                  select new
        //                                  {
        //                                      TBBETs.IDENTIFIANTBET,
        //                                      TBBETs.BRANCHEMENT_ID,
        //                                      TBBETs.CONTRATBTA_ID,
        //                                      TBBETs.TRAITEMENT_BET,
        //                                      TBBETs.PERIODEFACT_AN,
        //                                      TBBETs.PERIODEFACT_MOIS,
        //                                      TBBETs.NUMERO_COMPTEUR,
        //                                      TBBETs.CODEEXPLOITATION,
        //                                      TBBETs.CONSOMMATION,
        //                                      TBBETs.CONSOMOYENNE,
        //                                      TBBETs.INDEXRELA,
        //                                      TBBETs.INDEXNVL,
        //                                      TBBETs.CODANO_1,
        //                                      TBBETs.CODANO_2,
        //                                      TBBETs.CODANO_3,
        //                                      TBBETs.CODANO_4,
        //                                      TBBETs.CODANO_5,
        //                                      REFUNITEORGANISATIONNELLE.CODEUO,
        //                                      TBBETs.MATRICULEAGENTZONE,
        //                                      NOMAGENTINTERVENTION = ADMAGENTS.Nom,
        //                                      PRENOMSAGENTINTERVENTION = ADMAGENTS.Prenoms,
        //                                      TBBETs.MATRICULEAGENTMODIFICATION,
        //                                      NOMAGENTMODIFICATION = AdmAgentModification.Nom,
        //                                      PRENOMSAGENTMODIFICATION = AdmAgentModification.Prenoms,
        //                                      LIBELLECODEANO1 = LibelleCodeano_1.LIBELLEANAMOLIE,
        //                                      LIBELLECODEANO2 = LibelleCodeano_2.LIBELLEANAMOLIE,
        //                                      LIBELLECODEANO3 = LibelleCodeano_3.LIBELLEANAMOLIE,
        //                                      LIBELLECODEANO4 = LibelleCodeano_4.LIBELLEANAMOLIE,
        //                                      LIBELLECODEANO5 = LibelleCodeano_5.LIBELLEANAMOLIE,
        //                                      TBBETs.COMMENTAIRES
        //                                  }; ;
        //            IEnumerable<object> query =
        //              from StatistiquesBETs in StatistiquesBET
        //              join TBCONTRATBTAs in context.TBCONTRATBTA on new { ContratBTA_ID = (Guid)StatistiquesBETs.CONTRATBTA_ID } equals new { ContratBTA_ID = TBCONTRATBTAs.CONTRAT_ID }
        //              join TBTIERSCLIENTs in context.TBTIERSCLIENT
        //                    on new { TBCONTRATBTAs.CODEEXPLOITATION, TBCONTRATBTAs.REFERENCECLIENT }
        //                equals new { TBTIERSCLIENTs.CODEEXPLOITATION, TBTIERSCLIENTs.REFERENCECLIENT }
        //              join REFTYPEABONNEs in context.REFTYPEABONNE on TBTIERSCLIENTs.TYPABON_ID equals REFTYPEABONNEs.TYPABON_ID
        //              where
        //                StatistiquesBETs.TRAITEMENT_BET == false
        //              group new { StatistiquesBETs, TBCONTRATBTAs, TBTIERSCLIENTs, REFTYPEABONNEs } by new
        //              {
        //                  StatistiquesBETs.CODEEXPLOITATION,
        //                  StatistiquesBETs.CODEUO,
        //                  StatistiquesBETs.PERIODEFACT_AN,
        //                  StatistiquesBETs.PERIODEFACT_MOIS,
        //                  StatistiquesBETs.IDENTIFIANTBET,
        //                  StatistiquesBETs.BRANCHEMENT_ID,
        //                  StatistiquesBETs.NOMAGENTINTERVENTION,
        //                  StatistiquesBETs.PRENOMSAGENTINTERVENTION,
        //                  StatistiquesBETs.NOMAGENTMODIFICATION,
        //                  StatistiquesBETs.PRENOMSAGENTMODIFICATION,
        //                  StatistiquesBETs.TRAITEMENT_BET,
        //                  StatistiquesBETs.MATRICULEAGENTZONE,
        //                  StatistiquesBETs.MATRICULEAGENTMODIFICATION,
        //                  StatistiquesBETs.NUMERO_COMPTEUR,
        //                  StatistiquesBETs.CONSOMMATION,
        //                  StatistiquesBETs.CONSOMOYENNE,
        //                  StatistiquesBETs.INDEXRELA,
        //                  StatistiquesBETs.INDEXNVL,
        //                  StatistiquesBETs.CODANO_1,
        //                  StatistiquesBETs.CODANO_2,
        //                  StatistiquesBETs.CODANO_3,
        //                  StatistiquesBETs.CODANO_4,
        //                  StatistiquesBETs.CODANO_5,
        //                  StatistiquesBETs.CONTRATBTA_ID,
        //                  StatistiquesBETs.LIBELLECODEANO1,
        //                  StatistiquesBETs.LIBELLECODEANO2,
        //                  StatistiquesBETs.LIBELLECODEANO3,
        //                  StatistiquesBETs.LIBELLECODEANO4,
        //                  StatistiquesBETs.LIBELLECODEANO5,
        //                  TBCONTRATBTAs.REFERENCECLIENT,
        //                  TBCONTRATBTAs.STATUTCONTRAT,
        //                  TBCONTRATBTAs.GROUPEDEFACTURATION,
        //                  TBCONTRATBTAs.PUISSANCESOUSCRITE,
        //                  TBCONTRATBTAs.DATEABONNEMENT,
        //                  TBCONTRATBTAs.DATESTATUTCONTRAT,
        //                  TBTIERSCLIENTs.NOM,
        //                  TBTIERSCLIENTs.PRENOMS,
        //                  TBTIERSCLIENTs.TYPABON_ID,
        //                  REFTYPEABONNEs.TYPABON_LIBELLE,
        //                  StatistiquesBETs.COMMENTAIRES
        //              } into g
        //              select new
        //              {
        //                  g.Key.CODEEXPLOITATION,
        //                  g.Key.CODEUO,
        //                  Periodefact_An = (int?)g.Key.PERIODEFACT_AN,
        //                  PeriodeFact_Mois = (int?)g.Key.PERIODEFACT_MOIS,
        //                  g.Key.IDENTIFIANTBET,
        //                  g.Key.BRANCHEMENT_ID,
        //                  ContratBTA_ID = (Guid?)g.Key.CONTRATBTA_ID,
        //                  g.Key.MATRICULEAGENTZONE,
        //                  g.Key.NOMAGENTINTERVENTION,
        //                  g.Key.PRENOMSAGENTINTERVENTION,
        //                  g.Key.MATRICULEAGENTMODIFICATION,
        //                  g.Key.NOMAGENTMODIFICATION,
        //                  g.Key.PRENOMSAGENTMODIFICATION,
        //                  Traitement_BET = (bool?)g.Key.TRAITEMENT_BET,
        //                  g.Key.NUMERO_COMPTEUR,
        //                  Consommation = (int?)g.Key.CONSOMMATION,
        //                  ConsoMoyenne = (int?)g.Key.CONSOMOYENNE,
        //                  g.Key.INDEXRELA,
        //                  IndexNvl = (int?)g.Key.INDEXNVL,
        //                  g.Key.CODANO_1,
        //                  g.Key.CODANO_2,
        //                  g.Key.CODANO_3,
        //                  g.Key.CODANO_4,
        //                  g.Key.CODANO_5,
        //                  g.Key.LIBELLECODEANO1,
        //                  g.Key.LIBELLECODEANO2,
        //                  g.Key.LIBELLECODEANO3,
        //                  g.Key.LIBELLECODEANO4,
        //                  g.Key.LIBELLECODEANO5,
        //                  g.Key.REFERENCECLIENT,
        //                  STATUTCONTRAT = (int?)g.Key.STATUTCONTRAT,
        //                  GROUPEDEFACTURATION = (int?)g.Key.GROUPEDEFACTURATION,
        //                  PUISSANCESOUSCRITE = (double?)g.Key.PUISSANCESOUSCRITE,
        //                  DATEABONNEMENT = (DateTime?)g.Key.DATEABONNEMENT,
        //                  DATESTATUTCONTRAT = (DateTime?)g.Key.DATESTATUTCONTRAT,
        //                  g.Key.NOM,
        //                  g.Key.PRENOMS,
        //                  g.Key.TYPABON_ID,
        //                  g.Key.TYPABON_LIBELLE,
        //                  g.Key.COMMENTAIRES,
        //                  StatutContrat_Libelle =
        //                  (Int64)g.Key.STATUTCONTRAT == 1 ? "Actif" :
        //                  (Int64)g.Key.STATUTCONTRAT == 2 ? "Résilié" : ""
        //              };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwStatistiquesDeTraitementDesBETs()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            IEnumerable<object> query =
        //              from TBBETs in context.TBBET
        //              join REFEXPLOITATION in context.REFEXPLOITATION on new { CODEEXPLOITATION = (string)TBBETs.CODEEXPLOITATION } equals new { CODEEXPLOITATION = REFEXPLOITATION.CODEEXPLOITATION }
        //              from REFUNITEORGANISATIONNELLE in REFEXPLOITATION.REFUNITEORGANISATIONNELLE
        //              group new { TBBETs, REFEXPLOITATION } by new
        //              {
        //                  TBBETs.CODEEXPLOITATION,
        //                  TBBETs.PERIODEFACT_AN,
        //                  TBBETs.PERIODEFACT_MOIS,
        //                  REFUNITEORGANISATIONNELLE.CODEUO
        //              } into g
        //              select new
        //              {
        //                  g.Key.CODEUO,
        //                  g.Key.CODEEXPLOITATION,
        //                  PERIODEFACT_AN = (int?)g.Key.PERIODEFACT_AN,
        //                  PERIODEFACT_MOIS = (int?)g.Key.PERIODEFACT_MOIS,
        //                  NombreDeBETs = g.Count(p => p.TBBETs.TRAITEMENT_BET != null),
        //                  NbreBETTraites = g.Sum(p => (
        //                  p.TBBETs.TRAITEMENT_BET == true ? 1 : 0))
        //              };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwSynchronisation_Queue()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            IEnumerable<object> query =
        //              from SYNCHRONISATION_QUEUE in context.SYNCHRONISATION_QUEUE
        //              join REFEXPLOITATIONs in context.REFEXPLOITATION on SYNCHRONISATION_QUEUE.CODEEXPLOITATION equals REFEXPLOITATIONs.CODEEXPLOITATION
        //              select new
        //              {
        //                  SYNCHRONISATION_QUEUE.CODEEXPLOITATION,
        //                  SYNCHRONISATION_QUEUE.MATRICULEUSER,
        //                  SYNCHRONISATION_QUEUE.NOMMACHINE,
        //                  SYNCHRONISATION_QUEUE.ESTSYNCHROMANULLE,
        //                  SYNCHRONISATION_QUEUE.DATEINITIALISATIONDEMANDE,
        //                  SYNCHRONISATION_QUEUE.ETAPESYNCHRO,
        //                  SYNCHRONISATION_QUEUE.ARRETDEMANDE,
        //                  REFEXPLOITATIONs.EXPLOTATION_LIBELLE,
        //                  REFEXPLOITATIONs.SITEGESABEL
        //              };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwTourneeAccessiblesParlUO()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            IEnumerable<object> query =
        //              from TBTOURNEEs in context.TBTOURNEE
        //              join REFEXPLOITATION in context.REFEXPLOITATION on new { CODEEXPLOITATION = (string)TBTOURNEEs.CODEEXPLOITATION } equals new { CODEEXPLOITATION = REFEXPLOITATION.CODEEXPLOITATION }
        //              from REFUNITEORGANISATIONNELLE in REFEXPLOITATION.REFUNITEORGANISATIONNELLE
        //              select new
        //              {
        //                  TBTOURNEEs.TOURNEE_ID,
        //                  TBTOURNEEs.CODETOURNEE,
        //                  TBTOURNEEs.CODEEXPLOITATION,
        //                  TBTOURNEEs.CODEZONE,
        //                  TBTOURNEEs.LIBELLETOURNEE,
        //                  REFUNITEORGANISATIONNELLE.CODEUO
        //              };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwUOExploitation()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            IEnumerable<object> query =
        //              from REFEXPLOITATION in context.REFEXPLOITATION
        //              join REFEXPLOITATIONs in context.REFEXPLOITATION on new { CODEEXPLOITATION = (string)REFEXPLOITATION.CODEEXPLOITATION } equals new { CODEEXPLOITATION = REFEXPLOITATIONs.CODEEXPLOITATION }
        //              from REFUNITEORGANISATIONNELLE in REFEXPLOITATION.REFUNITEORGANISATIONNELLE
        //              select new
        //              {
        //                  REFUNITEORGANISATIONNELLE.CODEUO,
        //                  REFEXPLOITATIONs.CODEEXPLOITATION,
        //                  REFEXPLOITATIONs.EXPLOTATION_LIBELLE,
        //                  REFEXPLOITATIONs.DIRECTIONREGIONALE,
        //                  REFEXPLOITATIONs.SITEGESABEL
        //              };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwConsoBTA_HistoriqueBET()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            IEnumerable<object> query =
        //    from BET in
        //        (
        //            ((from TBBETs in context.TBBET
        //              select new
        //              {
        //                  TBBETs.CONTRATBTA_ID
        //              }).Distinct()))
        //    join Conso in context.TBCONSOMMATIONBTA on new { CONTRATBTA_ID = (Guid)BET.CONTRATBTA_ID } equals new { CONTRATBTA_ID = Conso.CONTRAT_ID }
        //    select new
        //    {
        //        Conso.VALEURCONSO,
        //        CONTRAT_ID = (Guid?)Conso.CONTRAT_ID,
        //        Conso.CODEEXPLOITATION,
        //        Conso.CODETYPECONSO,
        //        Conso.PERIODEFACT_AN,
        //        Conso.PERIODEFACT_MOIS,
        //        Conso.NBREDEJOURSDECONSO,
        //        Conso.INDEXANC,
        //        Conso.INDEXNVL,
        //        Conso.CONSOMMATION_ID,
        //        Conso.NUMERO_COMPTEUR,
        //        Conso.DATEFACT
        //    };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwConsoHTA_ElementDeCampagneHTA()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            var ElementsLotDeControleCampagneHTA = from TBLOTDECONTROLEHTAs in context.TBLOTDECONTROLEHTA
        //                                                   join TBELEMENTLOTDECONTROLEHTAs in context.TBELEMENTLOTDECONTROLEHTA on TBLOTDECONTROLEHTAs.LOT_ID equals TBELEMENTLOTDECONTROLEHTAs.LOT_ID
        //                                                   join Resultat in context.TBRESULTATCONTROLEHTA on new { RESULTATCONTROLE_ID = (Guid)TBELEMENTLOTDECONTROLEHTAs.RESULTATCONTROLE_ID } equals new { RESULTATCONTROLE_ID = Resultat.RESULTATCONTROLE_ID } into Resultat_join
        //                                                   from Resultat in Resultat_join.DefaultIfEmpty()
        //                                                   select new
        //                                                   {
        //                                                       LOT_ID = (Guid?)TBLOTDECONTROLEHTAs.LOT_ID,
        //                                                       TBLOTDECONTROLEHTAs.LIBELLE_LOT,
        //                                                       TBLOTDECONTROLEHTAs.STATUTLOT_ID,
        //                                                       TBLOTDECONTROLEHTAs.CAMPAGNE_ID,
        //                                                       TBLOTDECONTROLEHTAs.CRITERE_TYPECLIENT,
        //                                                       TBLOTDECONTROLEHTAs.NBREELEMENTSDULOT,
        //                                                       TBLOTDECONTROLEHTAs.MATRICULEAGENTCONTROLEUR,
        //                                                       TBLOTDECONTROLEHTAs.DATEFERMETURE,
        //                                                       TBLOTDECONTROLEHTAs.MATRICULEAGENTCREATION,
        //                                                       TBLOTDECONTROLEHTAs.CRITERE_TYPECOMPTEUR,
        //                                                       TBLOTDECONTROLEHTAs.DATECREATION,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.CONTRAT_ID,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.DATESELECTION,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.DATEAFFECTATIONLOT,
        //                                                       RESULTATCONTROLE_ID = (Guid?)Resultat.RESULTATCONTROLE_ID,
        //                                                       RESULTATVALUE = (int?)Resultat.RESULTATVALUE,
        //                                                       MATRICULEAGENTSAISIE = Resultat.MATRICULEAGENTSAISIE,
        //                                                       MATRICULEAGENTCONTROLE = Resultat.MATRICULEAGENTCONTROLE,
        //                                                       DATESAISIE = (DateTime?)Resultat.DATESAISIE,
        //                                                       DATECONTROLE = (DateTime?)Resultat.DATECONTROLE,
        //                                                       COMMENTAIRES = Resultat.COMMENTAIRES,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.METHODE_ID,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.DEBUT_PERAA,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.DEBUT_PERMM,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.FIN_PERAA,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.FIN_PERMM,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.DIFFERENCE,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.C_MCHUTECONSO_CONSOENCHUTE_VALEUR,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.C_MCHUTECONSO_CONSOENCHUTE_PERAA,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.C_MCHUTECONSO_CONSOENCHUTE_PERMM,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.C_MCHUTECONSO_CONSOPRECEDENTE_PERAA,
        //                                                       TBELEMENTLOTDECONTROLEHTAs.C_MCHUTECONSO_CONSOPRECEDENTE_PERMM
        //                                                   };

        //            var vwElementsDeCampagneHTA =
        //         from ElementsCampagneHTA in
        //             (
        //                 ((
        //                 from vwElementsLotDeControleCampagneHTAs in ElementsLotDeControleCampagneHTA
        //                 select new
        //                 {
        //                     CAMPAGNE_ID = (Guid?)vwElementsLotDeControleCampagneHTAs.CAMPAGNE_ID,
        //                     CONTRAT_ID = (Guid?)vwElementsLotDeControleCampagneHTAs.CONTRAT_ID,
        //                     DATESELECTION = (DateTime?)vwElementsLotDeControleCampagneHTAs.DATESELECTION,
        //                     LOT_ID = (Guid?)vwElementsLotDeControleCampagneHTAs.LOT_ID,
        //                     vwElementsLotDeControleCampagneHTAs.LIBELLE_LOT,
        //                     STATUTLOT_ID = (int?)vwElementsLotDeControleCampagneHTAs.STATUTLOT_ID,
        //                     DATECREATION = (DateTime?)vwElementsLotDeControleCampagneHTAs.DATECREATION,
        //                     vwElementsLotDeControleCampagneHTAs.MATRICULEAGENTCREATION,
        //                     vwElementsLotDeControleCampagneHTAs.MATRICULEAGENTCONTROLEUR,
        //                     NBREELEMENTSDULOT = (int?)vwElementsLotDeControleCampagneHTAs.NBREELEMENTSDULOT,
        //                     DATEFERMETURE = (DateTime?)vwElementsLotDeControleCampagneHTAs.DATEFERMETURE,
        //                     RESULTATCONTROLE_ID = (Guid?)vwElementsLotDeControleCampagneHTAs.RESULTATCONTROLE_ID,
        //                     RESULTATVALUE = (int?)vwElementsLotDeControleCampagneHTAs.RESULTATVALUE,
        //                     DATEAFFECTATIONLOT = (DateTime?)vwElementsLotDeControleCampagneHTAs.DATEAFFECTATIONLOT,
        //                     vwElementsLotDeControleCampagneHTAs.MATRICULEAGENTSAISIE,
        //                     DATESAISIE = (DateTime?)vwElementsLotDeControleCampagneHTAs.DATESAISIE,
        //                     DATECONTROLE = (DateTime?)vwElementsLotDeControleCampagneHTAs.DATECONTROLE,
        //                     vwElementsLotDeControleCampagneHTAs.COMMENTAIRES,
        //                     METHODE_ID = (int?)vwElementsLotDeControleCampagneHTAs.METHODE_ID,
        //                     DEBUT_PERAA = (int?)vwElementsLotDeControleCampagneHTAs.DEBUT_PERAA,
        //                     DEBUT_PERMM = (int?)vwElementsLotDeControleCampagneHTAs.DEBUT_PERMM,
        //                     FIN_PERAA = (int?)vwElementsLotDeControleCampagneHTAs.FIN_PERAA,
        //                     FIN_PERMM = (int?)vwElementsLotDeControleCampagneHTAs.FIN_PERMM,
        //                     DIFFERENCE = (double?)vwElementsLotDeControleCampagneHTAs.DIFFERENCE,
        //                     vwElementsLotDeControleCampagneHTAs.C_MCHUTECONSO_CONSOENCHUTE_VALEUR,
        //                     vwElementsLotDeControleCampagneHTAs.C_MCHUTECONSO_CONSOENCHUTE_PERAA,
        //                     vwElementsLotDeControleCampagneHTAs.C_MCHUTECONSO_CONSOENCHUTE_PERMM,
        //                     vwElementsLotDeControleCampagneHTAs.C_MCHUTECONSO_CONSOPRECEDENTE_PERAA,
        //                     vwElementsLotDeControleCampagneHTAs.C_MCHUTECONSO_CONSOPRECEDENTE_PERMM
        //                 }
        //                 ).Union
        //                 (
        //                 from TBELEMENTPRESELECTIONCAMPAGNEHTAs in context.TBELEMENTPRESELECTIONCAMPAGNEHTA
        //                 select new
        //                 {
        //                     CAMPAGNE_ID = (Guid?)TBELEMENTPRESELECTIONCAMPAGNEHTAs.CAMPAGNE_ID,
        //                     CONTRAT_ID = (Guid?)TBELEMENTPRESELECTIONCAMPAGNEHTAs.CONTRAT_ID,
        //                     DATESELECTION = (DateTime?)TBELEMENTPRESELECTIONCAMPAGNEHTAs.DATESELECTION,
        //                     LOT_ID = (Guid?)null,
        //                     LIBELLE_LOT = (String)null,
        //                     STATUTLOT_ID = (Int32?)null,
        //                     DATECREATION = (DateTime?)null,
        //                     MATRICULEAGENTCREATION = (String)null,
        //                     MATRICULEAGENTCONTROLEUR = (String)null,
        //                     NBREELEMENTSDULOT = (Int32?)null,
        //                     DATEFERMETURE = (DateTime?)null,
        //                     RESULTATCONTROLE_ID = (Guid?)null,
        //                     RESULTATVALUE = (Int32?)null,
        //                     DATEAFFECTATIONLOT = (DateTime?)null,
        //                     MATRICULEAGENTSAISIE = (String)null,
        //                     DATESAISIE = (DateTime?)null,
        //                     DATECONTROLE = (DateTime?)null,
        //                     COMMENTAIRES = (String)null,
        //                     METHODE_ID = (int?)TBELEMENTPRESELECTIONCAMPAGNEHTAs.METHODE_ID,
        //                     DEBUT_PERAA = (int?)TBELEMENTPRESELECTIONCAMPAGNEHTAs.DEBUT_PERAA,
        //                     DEBUT_PERMM = (int?)TBELEMENTPRESELECTIONCAMPAGNEHTAs.DEBUT_PERMM,
        //                     FIN_PERAA = (int?)TBELEMENTPRESELECTIONCAMPAGNEHTAs.FIN_PERAA,
        //                     FIN_PERMM = (int?)TBELEMENTPRESELECTIONCAMPAGNEHTAs.FIN_PERMM,
        //                     DIFFERENCE = (double?)TBELEMENTPRESELECTIONCAMPAGNEHTAs.DIFFERENCE,
        //                     TBELEMENTPRESELECTIONCAMPAGNEHTAs.C_MCHUTECONSO_CONSOENCHUTE_VALEUR,
        //                     TBELEMENTPRESELECTIONCAMPAGNEHTAs.C_MCHUTECONSO_CONSOENCHUTE_PERAA,
        //                     TBELEMENTPRESELECTIONCAMPAGNEHTAs.C_MCHUTECONSO_CONSOENCHUTE_PERMM,
        //                     TBELEMENTPRESELECTIONCAMPAGNEHTAs.C_MCHUTECONSO_CONSOPRECEDENTE_PERAA,
        //                     TBELEMENTPRESELECTIONCAMPAGNEHTAs.C_MCHUTECONSO_CONSOPRECEDENTE_PERMM
        //                 }
        //                 )))
        //         join TBCONTRATHTAs in context.TBCONTRATHTA on new { Contrat_ID = (Guid)ElementsCampagneHTA.CONTRAT_ID } equals new { Contrat_ID = TBCONTRATHTAs.CONTRAT_ID }
        //         join TBTIERSCLIENTs in context.TBTIERSCLIENT on TBCONTRATHTAs.REFERENCECLIENT equals TBTIERSCLIENTs.REFERENCECLIENT
        //         join TBRACCORDEMENTs in context.TBRACCORDEMENT on TBCONTRATHTAs.RACCORDEMENT_ID equals TBRACCORDEMENTs.RACCORDEMENT_ID
        //         join REFEXPLOITATION in context.REFEXPLOITATION on new { CODEEXPLOITATION = (string)TBCONTRATHTAs.CODEEXPLOITATION } equals new { CODEEXPLOITATION = REFEXPLOITATION.CODEEXPLOITATION }
        //         join TBCOMPTEURHTAs in context.TBCOMPTEURHTA
        //               on new { TBRACCORDEMENTs.NUMERO_COMPTEUR, TBRACCORDEMENTs.CODEEXPLOITATION }
        //           equals new { TBCOMPTEURHTAs.NUMERO_COMPTEUR, TBCOMPTEURHTAs.CODEEXPLOITATION } into TBCOMPTEURHTAs_join
       //         from TBCOMPTEURHTAs in TBCOMPTEURHTAs_join.DefaultIfEmpty()
        //         from REFUNITEORGANISATIONNELLE in REFEXPLOITATION.REFUNITEORGANISATIONNELLE
        //         select new
        //         {
        //             ElementsCampagneHTA.CAMPAGNE_ID,
        //             ElementsCampagneHTA.CONTRAT_ID,
        //             ElementsCampagneHTA.DATESELECTION,
        //             TBCONTRATHTAs.RACCORDEMENT_ID,
        //             TBTIERSCLIENTs.REFERENCECLIENT,
        //             ElementsCampagneHTA.LOT_ID,
        //             ElementsCampagneHTA.LIBELLE_LOT,
        //             ElementsCampagneHTA.STATUTLOT_ID,
        //             ElementsCampagneHTA.DATECREATION,
        //             ElementsCampagneHTA.MATRICULEAGENTCREATION,
        //             ElementsCampagneHTA.MATRICULEAGENTCONTROLEUR,
        //             ElementsCampagneHTA.NBREELEMENTSDULOT,
        //             ElementsCampagneHTA.DATEFERMETURE,
        //             REFUNITEORGANISATIONNELLE.CODEUO,
        //             TBTIERSCLIENTs.NOM,
        //             TBTIERSCLIENTs.PRENOMS,
        //             ElementsCampagneHTA.RESULTATCONTROLE_ID,
        //             ElementsCampagneHTA.RESULTATVALUE,
        //             ElementsCampagneHTA.MATRICULEAGENTSAISIE,
        //             ElementsCampagneHTA.DATECONTROLE,
        //             ElementsCampagneHTA.COMMENTAIRES,
        //             DateSaisieResultatControle = ElementsCampagneHTA.DATESAISIE,
        //             TBRACCORDEMENTs.NUMERO_COMPTEUR,
        //             ElementsCampagneHTA.DATEAFFECTATIONLOT,
        //             ElementsCampagneHTA.METHODE_ID,
        //             ElementsCampagneHTA.DEBUT_PERAA,
        //             ElementsCampagneHTA.DEBUT_PERMM,
        //             ElementsCampagneHTA.FIN_PERAA,
        //             ElementsCampagneHTA.FIN_PERMM,
        //             ElementsCampagneHTA.DIFFERENCE,
        //             ElementsCampagneHTA.C_MCHUTECONSO_CONSOENCHUTE_VALEUR,
        //             ElementsCampagneHTA.C_MCHUTECONSO_CONSOENCHUTE_PERAA,
        //             ElementsCampagneHTA.C_MCHUTECONSO_CONSOENCHUTE_PERMM,
        //             ElementsCampagneHTA.C_MCHUTECONSO_CONSOPRECEDENTE_PERAA,
        //             ElementsCampagneHTA.C_MCHUTECONSO_CONSOPRECEDENTE_PERMM,
        //             TBCONTRATHTAs.CODEEXPLOITATION,
        //             TBCONTRATHTAs.DATEDERNIERESELECTIONENCAMPAGNE
        //         };

        //            IEnumerable<object> query =
        //              from TBCONSOMMATIONHTAs in context.TBCONSOMMATIONHTA
        //              join vwElementsDeCampagneHTAs in vwElementsDeCampagneHTA on new { CONTRAT_ID = TBCONSOMMATIONHTAs.CONTRAT_ID } equals new { CONTRAT_ID = (Guid)vwElementsDeCampagneHTAs.CONTRAT_ID }
        //              select new
        //              {
        //                  vwElementsDeCampagneHTAs.CODEUO,
        //                  TBCONSOMMATIONHTAs.CONSOMMATION_ID,
        //                  TBCONSOMMATIONHTAs.VALEURCONSO,
        //                  CONTRAT_ID = (Guid?)TBCONSOMMATIONHTAs.CONTRAT_ID,
        //                  TBCONSOMMATIONHTAs.CODEEXPLOITATION,
        //                  TBCONSOMMATIONHTAs.DATEFACT,
        //                  TBCONSOMMATIONHTAs.PERIODEFACT_AN,
        //                  TBCONSOMMATIONHTAs.PERIODEFACT_MOIS,
        //                  TBCONSOMMATIONHTAs.CODETYPECONSO
        //              };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwConsosHTAAvecTrancheHoraireNulle()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            var AbonneHTA = from TBCONTRATHTAs in context.TBCONTRATHTA
        //                            join TBTIERSCLIENTs in context.TBTIERSCLIENT on TBCONTRATHTAs.REFERENCECLIENT equals TBTIERSCLIENTs.REFERENCECLIENT
        //                            join TBRACCORDEMENTs in context.TBRACCORDEMENT on TBCONTRATHTAs.RACCORDEMENT_ID equals TBRACCORDEMENTs.RACCORDEMENT_ID
        //                            join Resultat in context.TBRESULTATCONTROLEHTA on new { RESULTATDERNIERCONTROLE_ID = (Guid)TBCONTRATHTAs.RESULTATDERNIERCONTROLE_ID } equals new { RESULTATDERNIERCONTROLE_ID = Resultat.RESULTATCONTROLE_ID } into Resultat_join
        //                            from Resultat in Resultat_join.DefaultIfEmpty()
        //                            select new
        //                            {
        //                                TBTIERSCLIENTs.REFERENCECLIENT,
        //                                TBTIERSCLIENTs.TYPECLIENT,
        //                                TBTIERSCLIENTs.NOM,
        //                                TBTIERSCLIENTs.PRENOMS,
        //                                TBCONTRATHTAs.CONTRAT_ID,
        //                                TBCONTRATHTAs.RACCORDEMENT_ID,
        //                                RESULTATDERNIERCONTROLE_ID = (Guid?)TBCONTRATHTAs.RESULTATDERNIERCONTROLE_ID,
        //                                TBCONTRATHTAs.STATUTCONTRAT,
        //                                TBCONTRATHTAs.CODEEXPLOITATION,
        //                                TBRACCORDEMENTs.NUMERO_COMPTEUR,
        //                                TBRACCORDEMENTs.TYPECOMPTAGEHTA,
        //                                ResultatDernierControle_Value = (int?)Resultat.RESULTATVALUE,
        //                                MatriculeAgentDernierControle = Resultat.MATRICULEAGENTCONTROLE,
        //                                DateDernierControle = (DateTime?)Resultat.DATECONTROLE,
        //                                TBCONTRATHTAs.PUISSANCESOUSCRITE,
        //                                TBCONTRATHTAs.TYPETARIF_ID
        //                            };
        //            IEnumerable<object> query =
        //              from TBCONSOMMATIONHTAs in context.TBCONSOMMATIONHTA
        //              join AbonneHTAs in AbonneHTA on new { CONTRAT_ID = TBCONSOMMATIONHTAs.CONTRAT_ID } equals new { CONTRAT_ID = AbonneHTAs.CONTRAT_ID }
        //              where
        //                (Double)((Double?)TBCONSOMMATIONHTAs.CONSOACTIVEJ ?? (Double?)0) == 0 ||
        //                (Double)((Double?)TBCONSOMMATIONHTAs.CONSOACTIVEN ?? (Double?)0) == 0 ||
        //                (Double)((Double?)TBCONSOMMATIONHTAs.CONSOACTIVEP ?? (Double?)0) == 0
        //              select new
        //              {
        //                  AbonneHTAs.CODEEXPLOITATION,
        //                  AbonneHTAs.REFERENCECLIENT,
        //                  AbonneHTAs.NOM,
        //                  AbonneHTAs.PRENOMS,
        //                  Contrat_ID = (Guid?)AbonneHTAs.CONTRAT_ID,
        //                  AbonneHTAs.RACCORDEMENT_ID,
        //                  AbonneHTAs.STATUTCONTRAT,
        //                  TBCONSOMMATIONHTAs.PERIODEFACT_AN,
        //                  TBCONSOMMATIONHTAs.PERIODEFACT_MOIS,
        //                  TBCONSOMMATIONHTAs.VALEURCONSO,
        //                  TBCONSOMMATIONHTAs.CODETYPECONSO,
        //                  ConsoActiveJour = ((Double?)TBCONSOMMATIONHTAs.CONSOACTIVEJ ?? (Double?)0),
        //                  ConsoActiveNuit = ((Double?)TBCONSOMMATIONHTAs.CONSOACTIVEN ?? (Double?)0),
        //                  ConsoActivePointe = ((Double?)TBCONSOMMATIONHTAs.CONSOACTIVEP ?? (Double?)0)
        //              };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwDateDerniereSynchroParExploitation()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            IEnumerable<object> query =
        //              from REFEXPLOITATIONs in context.REFEXPLOITATION
        //              join TBSYNCHRONISATIONs in context.TBSYNCHRONISATION on REFEXPLOITATIONs.CODEEXPLOITATION equals TBSYNCHRONISATIONs.CODEEXPLOITATION into TBSYNCHRONISATIONs_join
        //              from TBSYNCHRONISATIONs in TBSYNCHRONISATIONs_join.DefaultIfEmpty()
        //              group new { REFEXPLOITATIONs, TBSYNCHRONISATIONs } by new
        //              {
        //                  REFEXPLOITATIONs.EXPLOTATION_LIBELLE,
        //                  REFEXPLOITATIONs.CODEEXPLOITATION
        //              } into g
        //              select new
        //              {
        //                  g.Key.CODEEXPLOITATION,
        //                  g.Key.EXPLOTATION_LIBELLE,
        //                  DateDerniereSynchro = (DateTime?)g.Max(p => p.TBSYNCHRONISATIONs.DATEFINSYNCHRONISATION)
        //              };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwEditionCRHTANonSaisis()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            IEnumerable<object> query =
        //              from TBCAMPAGNECONTROLEHTAs in context.TBCAMPAGNECONTROLEHTA
        //              join TBLOTDECONTROLEHTAs in context.TBLOTDECONTROLEHTA on TBCAMPAGNECONTROLEHTAs.CAMPAGNE_ID equals TBLOTDECONTROLEHTAs.CAMPAGNE_ID
        //              join TBELEMENTLOTDECONTROLEHTAs in context.TBELEMENTLOTDECONTROLEHTA on TBLOTDECONTROLEHTAs.LOT_ID equals TBELEMENTLOTDECONTROLEHTAs.LOT_ID
        //              join REFEXPLOITATION in context.REFEXPLOITATION on new { CODEEXPLOITATION = (string)TBCAMPAGNECONTROLEHTAs.CODEEXPLOITATION } equals new { CODEEXPLOITATION = REFEXPLOITATION.CODEEXPLOITATION }
        //              join TBCONTRATHTAs in context.TBCONTRATHTA on TBELEMENTLOTDECONTROLEHTAs.CONTRAT_ID equals TBCONTRATHTAs.CONTRAT_ID
        //              join TBTIERSCLIENTs in context.TBTIERSCLIENT on TBCONTRATHTAs.REFERENCECLIENT equals TBTIERSCLIENTs.REFERENCECLIENT
        //              join REFMETHODEDEDETECTIONCLIENTSHTAs in context.REFMETHODEDEDETECTIONCLIENTSHTA on TBELEMENTLOTDECONTROLEHTAs.METHODE_ID equals REFMETHODEDEDETECTIONCLIENTSHTAs.METHODE_ID
        //              from REFUNITEORGANISATIONNELLE in REFEXPLOITATION.REFUNITEORGANISATIONNELLE
        //              where
        //                TBELEMENTLOTDECONTROLEHTAs.RESULTATCONTROLE_ID == null
        //              select new
        //              {
        //                  TBCAMPAGNECONTROLEHTAs.LIBELLE_CAMPAGNE,
        //                  REFUNITEORGANISATIONNELLE.CODEUO,
        //                  TBCAMPAGNECONTROLEHTAs.CODEEXPLOITATION,
        //                  LOT_ID = (Guid?)TBLOTDECONTROLEHTAs.LOT_ID,
        //                  TBLOTDECONTROLEHTAs.LIBELLE_LOT,
        //                  TBLOTDECONTROLEHTAs.STATUTLOT_ID,
        //                  CAMPAGNE_ID = (Guid?)TBLOTDECONTROLEHTAs.CAMPAGNE_ID,
        //                  TBLOTDECONTROLEHTAs.DATECREATION,
        //                  TBLOTDECONTROLEHTAs.MATRICULEAGENTCONTROLEUR,
        //                  CONTRAT_ID = (Guid?)TBELEMENTLOTDECONTROLEHTAs.CONTRAT_ID,
        //                  TBELEMENTLOTDECONTROLEHTAs.RESULTATCONTROLE_ID,
        //                  TBCONTRATHTAs.RACCORDEMENT_ID,
        //                  TBTIERSCLIENTs.REFERENCECLIENT,
        //                  TBTIERSCLIENTs.NOM,
        //                  TBTIERSCLIENTs.PRENOMS,
        //                  METHODE_ID = (int?)REFMETHODEDEDETECTIONCLIENTSHTAs.METHODE_ID,
        //                  REFMETHODEDEDETECTIONCLIENTSHTAs.LIBELE_METHODE
        //              };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwEditionCRNonSaisis()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            IEnumerable<object> query =
        //              from TBCAMPAGNECONTROLEBTAs in context.TBCAMPAGNECONTROLEBTA
        //              join TBLOTDECONTROLEBTAs in context.TBLOTDECONTROLEBTA on TBCAMPAGNECONTROLEBTAs.CAMPAGNE_ID equals TBLOTDECONTROLEBTAs.CAMPAGNE_ID
        //              join TBELEMENTLOTDECONTROLEBTAs in context.TBELEMENTLOTDECONTROLEBTA on TBLOTDECONTROLEBTAs.LOT_ID equals TBELEMENTLOTDECONTROLEBTAs.LOT_ID
        //              join REFEXPLOITATION in context.REFEXPLOITATION on new { CODEEXPLOITATION = (string)TBCAMPAGNECONTROLEBTAs.CODEEXPLOITATION } equals new { CODEEXPLOITATION = REFEXPLOITATION.CODEEXPLOITATION }
        //              join TBCONTRATBTAs in context.TBCONTRATBTA on TBELEMENTLOTDECONTROLEBTAs.CONTRAT_ID equals TBCONTRATBTAs.CONTRAT_ID
        //              join TBTIERSCLIENTs in context.TBTIERSCLIENT on TBCONTRATBTAs.REFERENCECLIENT equals TBTIERSCLIENTs.REFERENCECLIENT
        //              join REFMETHODEDEDETECTIONCLIENTSBTAs in context.REFMETHODEDEDETECTIONCLIENTSBTA on TBELEMENTLOTDECONTROLEBTAs.METHODE_ID equals REFMETHODEDEDETECTIONCLIENTSBTAs.METHODE_ID
        //              from REFUNITEORGANISATIONNELLE in REFEXPLOITATION.REFUNITEORGANISATIONNELLE
        //              where
        //                TBELEMENTLOTDECONTROLEBTAs.RESULTATCONTROLE_ID == null
        //              select new
        //              {
        //                  TBCAMPAGNECONTROLEBTAs.LIBELLE_CAMPAGNE,
        //                  REFUNITEORGANISATIONNELLE.CODEUO,
        //                  TBCAMPAGNECONTROLEBTAs.CODEEXPLOITATION,
        //                  LOT_ID = (Guid?)TBLOTDECONTROLEBTAs.LOT_ID,
        //                  TBLOTDECONTROLEBTAs.LIBELLE_LOT,
        //                  TBLOTDECONTROLEBTAs.STATUTLOT_ID,
        //                  CAMPAGNE_ID = (Guid?)TBLOTDECONTROLEBTAs.CAMPAGNE_ID,
        //                  TBLOTDECONTROLEBTAs.DATECREATION,
        //                  TBLOTDECONTROLEBTAs.MATRICULEAGENTCONTROLEUR,
        //                  TBELEMENTLOTDECONTROLEBTAs.RESULTATCONTROLE_ID,
        //                  CONTRAT_ID = (Guid?)TBELEMENTLOTDECONTROLEBTAs.CONTRAT_ID,
        //                  TBCONTRATBTAs.BRANCHEMENT_ID,
        //                  TBTIERSCLIENTs.REFERENCECLIENT,
        //                  TBTIERSCLIENTs.NOM,
        //                  TBTIERSCLIENTs.PRENOMS,
        //                  METHODE_ID = (int?)REFMETHODEDEDETECTIONCLIENTSBTAs.METHODE_ID,
        //                  REFMETHODEDEDETECTIONCLIENTSBTAs.LIBELE_METHODE,
        //                  Expr1 = (Guid?)TBCAMPAGNECONTROLEBTAs.CAMPAGNE_ID
        //              };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ////public static DataTable VueHistoControlesBTAEnCampagne()
        ////{
        ////    try
        ////    {
        ////        using (RPNTEntities context = new RPNTEntities())
        ////        {
        ////            IEnumerable<object> query =
        ////           from TBELEMENTLOTDECONTROLEBTAs in context.TBELEMENTLOTDECONTROLEBTA
        ////           join TBRESULTATCONTROLEBTAs in context.TBRESULTATCONTROLEBTA on new { RESULTATCONTROLE_ID = (Guid)TBELEMENTLOTDECONTROLEBTAs.RESULTATCONTROLE_ID } equals new { RESULTATCONTROLE_ID = TBRESULTATCONTROLEBTAs.RESULTATCONTROLE_ID } into TBRESULTATCONTROLEBTAs_join
        ////           from TBRESULTATCONTROLEBTAs in TBRESULTATCONTROLEBTAs_join.DefaultIfEmpty()
        ////           select new
        ////           {
        ////               TBELEMENTLOTDECONTROLEBTAs.CONTRAT_ID,
        ////               RESULTATCONTROLE_ID = (Guid?)TBRESULTATCONTROLEBTAs.RESULTATCONTROLE_ID,
        ////               TBELEMENTLOTDECONTROLEBTAs.METHODE_ID,
        ////               TBELEMENTLOTDECONTROLEBTAs.DIFFERENCE,
        ////               RESULTATVALUE = (int?)TBRESULTATCONTROLEBTAs.RESULTATVALUE
        ////           };
        ////            return Galatee.Tools.Utility.ListToDataTable(query);
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////}

        ////public static DataTable VueHistoControlesHTAEnCampagneHTA()
        ////{
        ////    try
        ////    {
        ////        using (RPNTEntities context = new RPNTEntities())
        ////        {
        ////            IEnumerable<object> query =
        ////           from TBELEMENTLOTDECONTROLEHTAs in context.TBELEMENTLOTDECONTROLEHTA
        ////           join Resultat in context.TBRESULTATCONTROLEHTA on new { RESULTATCONTROLE_ID = (Guid)TBELEMENTLOTDECONTROLEHTAs.RESULTATCONTROLE_ID } equals new { RESULTATCONTROLE_ID = Resultat.RESULTATCONTROLE_ID } into Resultat_join
        ////           from Resultat in Resultat_join.DefaultIfEmpty()
        ////           select new
        ////           {
        ////               TBELEMENTLOTDECONTROLEHTAs.CONTRAT_ID,
        ////               TBELEMENTLOTDECONTROLEHTAs.METHODE_ID,
        ////               RESULTATCONTROLE_ID = (Guid?)Resultat.RESULTATCONTROLE_ID,
        ////               RESULTATVALUE = (int?)Resultat.RESULTATVALUE,
        ////               TBELEMENTLOTDECONTROLEHTAs.DIFFERENCE
        ////           };
        ////            return Galatee.Tools.Utility.ListToDataTable(query);
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////}

        //public static DataTable VuevwUtilisateur()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            IEnumerable<object> query =
        //            from ADMAGENTS in context.ADMAGENTS
        //            join ADMUSERS in context.ADMUSERS on ADMAGENTS.MATRICULEAGENT equals ADMUSERS.MATRICULEAGENT
        //            select new
        //            {
        //                ADMUSERS.LOGINNAME,
        //                ADMUSERS.PASSWORD,
        //                ADMAGENTS.MATRICULEAGENT,
        //                ADMAGENTS.Nom,
        //                ADMAGENTS.Prenoms,
        //                ADMAGENTS.DisplayName,
        //                ADMAGENTS.EMAIL,
        //                ADMUSERS.DATE_CREATION,
        //                ADMUSERS.DATE_DERNIERE_MODIFICATION,
        //                ADMUSERS.STATUT,
        //                ADMUSERS.INIT_USER_PASSWORD,
        //                ADMUSERS.DATEDERNIEREMODIFICATIONPASSWORD,
        //                ADMUSERS.DERNIERECONNEXIONREUSSIE,
        //                ADMUSERS.DATEDERNIERVERROUILLAGE,
        //                ADMUSERS.DATEDERNIERECONNEXION,
        //                ADMUSERS.NOMBREECHECSOUVERTURESESSION,
        //                ADMUSERS.CHIFFREMENTDUPASSWORDREVERSIBLE,
        //                ADMUSERS.ESTADMINSPECIAL
        //            };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwMenusAffectesAuRole()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            var SousMenusAffectesAuRole = from Menu in context.ADMMENU
        //                                          from Permission in Menu.ADMROLES
        //                                          select new
        //                                          {
        //                                              Permission.RoleID,
        //                                              Menu.MENUID,
        //                                              Menu.MENUTEXT,
        //                                              Menu.MAINMENUID,
        //                                              Menu.MENUORDER,
        //                                              Menu.ISACTIVE,
        //                                              Menu.FORMNAME,
        //                                              Menu.ISDOCK,
        //                                              Menu.ICONNAME,
        //                                              Menu.SOUSCONTROL
        //                                          };
        //            IEnumerable<object> query =
        //            (
        //                from MenuPrincipal in
        //                    (
        //                        ((from SousMenusAffectesAuRoles in SousMenusAffectesAuRole
        //                          select new
        //                          {
        //                              SousMenusAffectesAuRoles.RoleID,
        //                              SousMenusAffectesAuRoles.MAINMENUID
        //                          }).Distinct()))
        //                join ADMMENUs in context.ADMMENU on new { MainMenuID = (int)MenuPrincipal.MAINMENUID } equals new { MainMenuID = ADMMENUs.MENUID }
        //                select new
        //                {
        //                    RoleID = (Guid?)MenuPrincipal.RoleID,
        //                    MENUID = (int?)ADMMENUs.MENUID,
        //                    ADMMENUs.MENUTEXT,
        //                    MAINMENUID = (int?)ADMMENUs.MAINMENUID,
        //                    MENUORDER = (int?)ADMMENUs.MENUORDER,
        //                    ISACTIVE = (bool?)ADMMENUs.ISACTIVE,
        //                    ADMMENUs.FORMNAME,
        //                    ISDOCK = (bool?)ADMMENUs.ISDOCK,
        //                    ADMMENUs.ICONNAME,
        //                    ADMMENUs.SOUSCONTROL
        //                }
        //            ).Union
        //            (
        //                from SousMenu in SousMenusAffectesAuRole
        //                select new
        //                {
        //                    RoleID = (Guid?)SousMenu.RoleID,
        //                    MENUID = (int?)SousMenu.MENUID,
        //                    MENUTEXT = SousMenu.MENUTEXT,
        //                    MAINMENUID = (int?)SousMenu.MAINMENUID,
        //                    MENUORDER = (int?)SousMenu.MENUORDER,
        //                    ISACTIVE = (bool?)SousMenu.ISACTIVE,
        //                    FORMNAME = SousMenu.FORMNAME,
        //                    ISDOCK = (bool?)SousMenu.ISDOCK,
        //                    ICONNAME = SousMenu.ICONNAME,
        //                    SOUSCONTROL = SousMenu.SOUSCONTROL
        //                }
        //            );
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ////public static DataTable VueSousMenusAffectesAuRole()
        ////{
        ////    try
        ////    {
        ////        using (RPNTEntities context = new RPNTEntities())
        ////        {
        ////            IEnumerable<object> query =
        ////            from Menu in context.ADMMENU
        ////            from Permission in Menu.ADMROLES
        ////            select new
        ////            {
        ////                Permission.RoleID,
        ////                Menu.MENUID,
        ////                Menu.MENUTEXT,
        ////                Menu.MAINMENUID,
        ////                Menu.MENUORDER,
        ////                Menu.ISACTIVE,
        ////                Menu.FORMNAME,
        ////                Menu.ISDOCK,
        ////                Menu.ICONNAME,
        ////                Menu.SOUSCONTROL
        ////            };
        ////            return Galatee.Tools.Utility.ListToDataTable(query);
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////}

        //public static DataTable VuevwClientsHTAAvecPSNonConformeAuTC()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            var AbonneHTAAvecRaccordement = from TBCONTRATHTAs in context.TBCONTRATHTA
        //                                            join TBTIERSCLIENTs in context.TBTIERSCLIENT
        //                                                  on new { TBCONTRATHTAs.CODEEXPLOITATION, TBCONTRATHTAs.REFERENCECLIENT }
        //                                              equals new { TBTIERSCLIENTs.CODEEXPLOITATION, TBTIERSCLIENTs.REFERENCECLIENT }
        //                                            join TBRACCORDEMENTs in context.TBRACCORDEMENT on TBCONTRATHTAs.RACCORDEMENT_ID equals TBRACCORDEMENTs.RACCORDEMENT_ID
        //                                            select new
        //                                            {
        //                                                TBCONTRATHTAs.CODEEXPLOITATION,
        //                                                TBCONTRATHTAs.REFERENCECLIENT,
        //                                                TBTIERSCLIENTs.NOM,
        //                                                TBTIERSCLIENTs.PRENOMS,
        //                                                TBTIERSCLIENTs.TYPECLIENT,
        //                                                TBTIERSCLIENTs.TYPABON_ID,
        //                                                TBCONTRATHTAs.RACCORDEMENT_ID,
        //                                                TBRACCORDEMENTs.TYPECOMPTAGEHTA,
        //                                                TBCONTRATHTAs.STATUTCONTRAT,
        //                                                TBCONTRATHTAs.PUISSANCESOUSCRITE,
        //                                                TBCONTRATHTAs.TYPETARIF_ID,
        //                                                TBCONTRATHTAs.RAPPORTTC_INTENTREE,
        //                                                TBCONTRATHTAs.RAPPORTTC_INTSORTIE,
        //                                                TBCONTRATHTAs.NIVEAUTENSION_ID,
        //                                                TBCONTRATHTAs.CONTRAT_ID,
        //                                                TBCONTRATHTAs.RESULTATDERNIERCONTROLE_ID,
        //                                                TBCONTRATHTAs.DATEPROCHAINCONTROLEPOSTFRAUDE
        //                                            };
        //            var vwLigneABAQUE_ChoixTC = from REFABAQUE_CHOIXTC0 in context.REFABAQUE_CHOIXTC
        //                                        join REFABAQUE_CHOIXTC in context.REFABAQUE_CHOIXTC on new { ABAQUE_ID = (string)REFABAQUE_CHOIXTC0.ABAQUE_ID } equals new { ABAQUE_ID = REFABAQUE_CHOIXTC.ABAQUE_ID }
        //                                        join REFLIGNEABAQUE_CHOIXTC in context.REFLIGNEABAQUE_CHOIXTC on REFABAQUE_CHOIXTC.ABAQUE_ID equals REFLIGNEABAQUE_CHOIXTC.ABAQUE_ID
        //                                        from REFTYPECOMPTAGEHTA in REFABAQUE_CHOIXTC0.REFTYPECOMPTAGEHTA
        //                                        select new
        //                                        {
        //                                            REFTYPECOMPTAGEHTA.TYPECOMPTAGEHTA_ID,
        //                                            REFABAQUE_CHOIXTC.NIVEAUTENSION_ID,
        //                                            REFLIGNEABAQUE_CHOIXTC.ABAQUE_ID,
        //                                            REFLIGNEABAQUE_CHOIXTC.PS_BORNEINF,
        //                                            REFLIGNEABAQUE_CHOIXTC.PS_BORNESUP,
        //                                            REFLIGNEABAQUE_CHOIXTC.RAPPORTTC_INTENTREE,
        //                                            REFLIGNEABAQUE_CHOIXTC.RAPPORTTC_INTSORTIE
        //                                        };

        //            IEnumerable<object> query =
        //           from Abonne in AbonneHTAAvecRaccordement
        //           join vwLigneABAQUE_ChoixTCs in vwLigneABAQUE_ChoixTC on new { TypeComptageHTA = (int)Abonne.TYPECOMPTAGEHTA } equals new { TypeComptageHTA = vwLigneABAQUE_ChoixTCs.TYPECOMPTAGEHTA_ID }
        //           join REFTYPECOMPTAGEHTAs in context.REFTYPECOMPTAGEHTA on new { TypeComptageHTA = (int)Abonne.TYPECOMPTAGEHTA } equals new { TypeComptageHTA = REFTYPECOMPTAGEHTAs.TYPECOMPTAGEHTA_ID }
        //           where
        //             Abonne.PUISSANCESOUSCRITE >= vwLigneABAQUE_ChoixTCs.PS_BORNEINF &&
        //             Abonne.PUISSANCESOUSCRITE <= vwLigneABAQUE_ChoixTCs.PS_BORNESUP &&
        //             vwLigneABAQUE_ChoixTCs.NIVEAUTENSION_ID == Abonne.NIVEAUTENSION_ID &&
        //             Abonne.RAPPORTTC_INTENTREE != vwLigneABAQUE_ChoixTCs.RAPPORTTC_INTENTREE
        //           select new
        //           {
        //               Abonne.CODEEXPLOITATION,
        //               Abonne.REFERENCECLIENT,
        //               TYPECOMPTAGEHTA = (int?)Abonne.TYPECOMPTAGEHTA,
        //               Abonne.PUISSANCESOUSCRITE,
        //               Abonne.NIVEAUTENSION_ID,
        //               TCClient_INTENTREE = Abonne.RAPPORTTC_INTENTREE,
        //               TCClient_INTSORTIE = Abonne.RAPPORTTC_INTSORTIE,
        //               vwLigneABAQUE_ChoixTCs.ABAQUE_ID,
        //               AbaqueTC_INTENTREE = vwLigneABAQUE_ChoixTCs.RAPPORTTC_INTENTREE,
        //               AbaqueTC_INTSORTIE = vwLigneABAQUE_ChoixTCs.RAPPORTTC_INTSORTIE,
        //               Abonne.NOM,
        //               Abonne.PRENOMS,
        //               Abonne.STATUTCONTRAT,
        //               Abonne.CONTRAT_ID,
        //               Abonne.RACCORDEMENT_ID,
        //               REFTYPECOMPTAGEHTAs.TYPECOMPTAGEHTA_LIBELLE
        //           };
        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwListeControlesPostFraudeBTAEchus()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //            var AbonneBTA = from TBCONTRATBTAs in context.TBCONTRATBTA
        //                         join TBTIERSCLIENTs in context.TBTIERSCLIENT
        //                               on new { TBCONTRATBTAs.CODEEXPLOITATION, TBCONTRATBTAs.REFERENCECLIENT }
        //                           equals new { TBTIERSCLIENTs.CODEEXPLOITATION, TBTIERSCLIENTs.REFERENCECLIENT }
        //                         select new
        //                         {
        //                             TBTIERSCLIENTs.CODEEXPLOITATION,
        //                             TBTIERSCLIENTs.REFERENCECLIENT,
        //                             TBTIERSCLIENTs.TYPECLIENT,
        //                             TBTIERSCLIENTs.TYPABON_ID,
        //                             TBTIERSCLIENTs.NOM,
        //                             TBTIERSCLIENTs.PRENOMS,
        //                             TBCONTRATBTAs.CONTRAT_ID,
        //                             TBCONTRATBTAs.BRANCHEMENT_ID,
        //                             TBCONTRATBTAs.RESULTATDERNIERCONTROLE_ID,
        //                             TBCONTRATBTAs.STATUTCONTRAT,
        //                             TBCONTRATBTAs.GROUPEDEFACTURATION,
        //                             TBCONTRATBTAs.PUISSANCESOUSCRITE,
        //                             TBCONTRATBTAs.TYPECONTRATID,
        //                             TBCONTRATBTAs.TYPETARIF_ID,
        //                             TBCONTRATBTAs.USAGEABONNEMENT_ID,
        //                             TBCONTRATBTAs.DATEABONNEMENT,
        //                             TBCONTRATBTAs.DATESTATUTCONTRAT,
        //                             TBCONTRATBTAs.DATEPROCHAINCONTROLEPOSTFRAUDE,
        //                             TBCONTRATBTAs.CONSTATDUCONTROLEPOSTFRAUDE_ID
        //                         };

        //            IEnumerable<object> query  =
        //            from TypeAnomalie in context.REFTYPEANOMALIEBTA
        //            from Constat in TypeAnomalie.TBCONSTATFRAUDEBTA
        //            join Abonne in AbonneBTA on new { CONTRAT_ID =  (Guid)Constat.CONTRAT_ID, Constat.CONSTATFRAUDE_ID }
        //            equals new { Abonne.CONTRAT_ID, CONSTATFRAUDE_ID = (Guid)Abonne.CONSTATDUCONTROLEPOSTFRAUDE_ID }
        //            select new
        //            {
        //                Abonne.CODEEXPLOITATION,
        //                Abonne.CONTRAT_ID,
        //                Abonne.REFERENCECLIENT,
        //                Abonne.NOM,
        //                Abonne.PRENOMS,
        //                Abonne.BRANCHEMENT_ID,
        //                Abonne.DATEPROCHAINCONTROLEPOSTFRAUDE,
        //                Constat.DATECONTROLE,
        //                Constat.MATRICULEAGENTSAISIE,
        //                Constat.DATESAISIE,
        //                Abonne.RESULTATDERNIERCONTROLE_ID,
        //                Constat.ACTIONSDURANTLECONSTAT,
        //                Constat.MATRICULEAGENTCONTROLEUR, 
        //                Constat.CONSTATFRAUDE_ID,
        //                Constat.NUMERO_AVISDEPASSAGE,
        //                TypeAnomalie.TYPEANOMALIE_ID
        //            };

        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable VuevwListeControlesPostFraudeHTAEchus()
        //{
        //    try
        //    {
        //        using (RPNTEntities context = new RPNTEntities())
        //        {
        //           var AbonneHTA = from TBCONTRATHTAs in context.TBCONTRATHTA
        //                            join TBTIERSCLIENTs in context.TBTIERSCLIENT
        //                                  on new { TBCONTRATHTAs.CODEEXPLOITATION, TBCONTRATHTAs.REFERENCECLIENT }
        //                              equals new { TBTIERSCLIENTs.CODEEXPLOITATION, TBTIERSCLIENTs.REFERENCECLIENT }
        //                            select new
        //                            {
        //                                TBCONTRATHTAs.CODEEXPLOITATION,
        //                                TBCONTRATHTAs.REFERENCECLIENT,
        //                                TBTIERSCLIENTs.NOM,
        //                                TBTIERSCLIENTs.PRENOMS,
        //                                TBTIERSCLIENTs.TYPECLIENT,
        //                                TBTIERSCLIENTs.TYPABON_ID,
        //                                TBCONTRATHTAs.RACCORDEMENT_ID,
        //                                TBCONTRATHTAs.STATUTCONTRAT,
        //                                TBCONTRATHTAs.PUISSANCESOUSCRITE,
        //                                TBCONTRATHTAs.TYPETARIF_ID,
        //                                TBCONTRATHTAs.RAPPORTTC_INTENTREE,
        //                                TBCONTRATHTAs.RAPPORTTC_INTSORTIE,
        //                                TBCONTRATHTAs.NIVEAUTENSION_ID,
        //                                TBCONTRATHTAs.CONTRAT_ID,
        //                                TBCONTRATHTAs.RESULTATDERNIERCONTROLE_ID,
        //                                TBCONTRATHTAs.DATEPROCHAINCONTROLEPOSTFRAUDE,
        //                                TBCONTRATHTAs.CONSTATDUCONTROLEPOSTFRAUDE_ID
        //                            };
        //           IEnumerable<object> query =
        //           from TypeAnomalie in context.REFTYPEANOMALIEHTA
        //           from Constat in TypeAnomalie.TBCONSTATFRAUDEHTA
        //           join Abonne in AbonneHTA on new { CONTRAT_ID = (Guid)Constat.CONTRATHTA_ID, Constat.CONSTATFRAUDE_ID }
        //           equals new { Abonne.CONTRAT_ID, CONSTATFRAUDE_ID = (Guid)Abonne.CONSTATDUCONTROLEPOSTFRAUDE_ID }
        //           select new
        //           {
        //               Abonne.CODEEXPLOITATION,
        //               Abonne.CONTRAT_ID,
        //               Abonne.REFERENCECLIENT,
        //               Abonne.NOM,
        //               Abonne.PRENOMS,
        //               Abonne.DATEPROCHAINCONTROLEPOSTFRAUDE,
        //               Constat.DATECONTROLE,
        //               Constat.MATRICULEAGENTSAISIE,
        //               Constat.DATESAISIE,
        //               Abonne.RESULTATDERNIERCONTROLE_ID,
        //               Constat.ACTIONSDURANTLECONSTAT,
        //               Constat.MATRICULEAGENTCONTROLEUR,
        //               Constat.CONSTATFRAUDE_ID,
        //               Constat.NUMERO_AVISDEPASSAGE,
        //               TypeAnomalie.TYPEANOMALIE_ID
        //           };

        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //#endregion

        //#region CAMPAGNE
           
        //    #region Methode de detection

        //        #region BTA

        public List<CsRefMethodesDeDetectionClientsBTA> GetMethodeDetectionBTA()
        {
            try
            {
                using (var context = new galadbEntities())
                {
                    List<CsRefMethodesDeDetectionClientsBTA> ListeMethode = new List<CsRefMethodesDeDetectionClientsBTA>();
                    foreach (var item in context.RefMethodesDeDetectionClientsBTA)
                    {
                      CsRefMethodesDeDetectionClientsBTA Methode=  new CsRefMethodesDeDetectionClientsBTA
                        {
                            Description = item.Description,
                            Libele_Methode = item.Libele_Methode,
                            Methode_ID = item.Methode_ID
                        };

                      Methode.ListParametres = new List<CstbParametres>();
                      foreach (var item_ in item.tbParametres)
                      {
                          CstbParametres Param = new CstbParametres
                          {
                              Description = item_.Description,
                              LibelleParametre = item_.LibelleParametre,
                              Methode_ID = item_.Methode_ID,
                              Parametre_ID = item_.Parametre_ID,
                              TypeValeur = item_.TypeValeur,
                              ValeurGlobaleChaine = item_.ValeurGlobaleChaine,
                              ValeurGlobaleDecimal = item_.ValeurGlobaleDecimal,
                              ValeurGlobaleInt = item_.ValeurGlobaleInt
                          };

                          Methode.ListParametres.Add(Param);
                      }
                      ListeMethode.Add(Methode);
                    }
                   

                    return ListeMethode;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //            public bool SaveMethodeDetectionBTA(CsREFMETHODEDEDETECTIONCLIENTSBTA MthDetection, bool IsUpdate)
        //            {

        //                try
        //                {
        //                    using (var context = new galadbEntities())
        //                    {
        //                        REFMETHODEDEDETECTIONCLIENTSBTA Mth_dect = context.REFMETHODEDEDETECTIONCLIENTSBTA.FirstOrDefault(md => md.METHODE_ID == MthDetection.METHODE_ID);
        //                        var m_d = context.TBMethodeDedectectionClientsBTA_PARAMETRE.Where(mth_detec => mth_detec.MethDection == MthDetection.METHODE_ID).ToList();
        //                        REFMETHODEDEDETECTIONCLIENTSBTA new_mth_detect;
        //                        //Si IsUpdate est à true on fait une mise à jour
        //                        if (IsUpdate)
        //                        {

        //                            Mth_dect.LIBELE_METHODE = MthDetection.LIBELE_METHODE;
        //                            Mth_dect.DESCRIPTION = MthDetection.DESCRIPTION;
        //                            //Mth_dect.TBMethodeDedectectionClientsBTA_PARAMETRE = m_d;
        //                            foreach (var item in MthDetection.tag)
        //                            {

        //                                if (item.MethDectParamatre_ID == 0)
        //                                {
        //                                    context.TBMethodeDedectectionClientsBTA_PARAMETRE.Add(new TBMethodeDedectectionClientsBTA_PARAMETRE
        //                                    {
        //                                        //MethDectParamatre_ID=int.MinValue,
        //                                        Parametre = item.Parametre,
        //                                        MethDection = MthDetection.METHODE_ID
        //                                    });
        //                                }
        //                                else
        //                                {
        //                                    m_d.FirstOrDefault(p => p.MethDectParamatre_ID == item.MethDectParamatre_ID).Parametre = item.Parametre;
        //                                    m_d.FirstOrDefault(p => p.MethDectParamatre_ID == item.MethDectParamatre_ID).MethDection = MthDetection.METHODE_ID;
        //                                }
        //                            }
        //                            List<int> temp_md_id = new List<int>(); ;
        //                            foreach (var item in m_d)
        //                            {
        //                                if (MthDetection.tag.Select(p => p.MethDectParamatre_ID).Contains(item.MethDectParamatre_ID) != true)
        //                                {
        //                                    temp_md_id.Add(m_d.FirstOrDefault(c => c.MethDectParamatre_ID == item.MethDectParamatre_ID).MethDectParamatre_ID);
        //                                }
        //                            }
        //                            foreach (var item in temp_md_id)
        //                            {
        //                                context.TBMethodeDedectectionClientsBTA_PARAMETRE.Remove(m_d.FirstOrDefault(c => c.MethDectParamatre_ID == item));
        //                            }


        //                        }
        //                        else
        //                        {
        //                            //Sinon une nouvelle insertion
        //                            new_mth_detect = new REFMETHODEDEDETECTIONCLIENTSBTA
        //                            {
        //                                DESCRIPTION = MthDetection.DESCRIPTION,
        //                                LIBELE_METHODE = MthDetection.LIBELE_METHODE
        //                                ,  TBMethodeDedectectionClientsBTA_PARAMETRE=m_d
        //                            };
        //                            context.REFMETHODEDEDETECTIONCLIENTSBTA.Add(new_mth_detect);
        //                            context.SaveChanges();

        //                            foreach (var item in MthDetection.tag)
        //                            {
        //                                if (item.MethDectParamatre_ID <= 0)
        //                                {
        //                                    m_d.Add(new TBMethodeDedectectionClientsBTA_PARAMETRE
        //                                    {
        //                                        Parametre = item.Parametre,
        //                                        MethDection = new_mth_detect.METHODE_ID,
        //                                    });
        //                                }

        //                            }

        //                        }

        //                        context.SaveChanges();

        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    throw ex;
        //                }



        //                return true;
        //            }
        //            public bool DeleteMethodeDetectionBTA(int Methode_Id)
        //            {
        //                // TODO :prévoir un top(champ donnant l'état d'une methode:actif,inactif,visible,invisible ect)pour eviter des incoherance dans d'autre traitements
        //                try
        //                {
        //                    using (var context = new galadbEntities())
        //                    {
        //                        foreach (var item in context.TBMethodeDedectectionClientsBTA_PARAMETRE.Where(md_p=>md_p.MethDection==Methode_Id))
        //                        {
        //                            context.TBMethodeDedectectionClientsBTA_PARAMETRE.Remove(item);
        //                        }
        //                        context.REFMETHODEDEDETECTIONCLIENTSBTA.Remove(context.REFMETHODEDEDETECTIONCLIENTSBTA.FirstOrDefault(md => md.METHODE_ID == Methode_Id));
        //                        context.SaveChanges();
        //                        return true;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    throw ex;
        //                }
        //            }

        //        #endregion

        //        #region HTA

        //            public List<CsREFMETHODEDEDETECTIONCLIENTSHTA> GetMethodeDetectionHTA()
        //            {
        //                try
        //                {
        //                    using (var rpntcontext = new galadbEntities())
        //                    {
        //                        var MTHDETEC_HAT = rpntcontext.REFMETHODEDEDETECTIONCLIENTSHTA;
        //                        return Entities.ConvertObject<CsREFMETHODEDEDETECTIONCLIENTSHTA, REFMETHODEDEDETECTIONCLIENTSHTA>(MTHDETEC_HAT.ToList());
        //                    }
        //                }
        //                catch (Exception ex)
        //                {

        //                    throw ex;
        //                }

        //            }

        //        #endregion

        //    #endregion

        //    #region Paramettre méthode de detection

        //        public List<CsTBPARAMETRE> LoadParametreMthDtect()
        //        {
        //            try
        //            {
        //                using (var context = new galadbEntities())
        //                {

        //                    return Entities.ConvertObject<CsTBPARAMETRE, TBPARAMETRE>(context.TBPARAMETRE.ToList());
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //        public bool SaveParamtreMethodeBTA(CsTBPARAMETRE paramtre, bool IsUpdate)
        //        {

        //            try
        //            {
        //                using (var context = new galadbEntities())
        //                {

        //                    if (IsUpdate)
        //                    {
        //                        TBPARAMETRE param = context.TBPARAMETRE.FirstOrDefault(p => p.PARAMETRE_ID == paramtre.PARAMETRE_ID);
        //                        param.LIBELLEPARAMETRE = paramtre.LIBELLEPARAMETRE;
        //                        param.DESCRIPTION = paramtre.DESCRIPTION;
        //                        param.VALEUR = paramtre.VALEUR;
        //                    }
        //                    else
        //                    {
        //                        context.TBPARAMETRE.Add(new TBPARAMETRE { LIBELLEPARAMETRE = paramtre.LIBELLEPARAMETRE, DESCRIPTION = paramtre.DESCRIPTION, VALEUR = paramtre.VALEUR });
        //                    }
        //                    context.SaveChanges();
        //                    return true;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }

        //        }
        //        public bool DeleteParamMethodeDetectionBTA(int param_id)
        //        {
        //            try
        //            {
        //                using (var context = new galadbEntities())
        //                {
        //                    context.TBPARAMETRE.Remove(context.TBPARAMETRE.FirstOrDefault(p => p.PARAMETRE_ID == param_id));
        //                    context.SaveChanges();
        //                    return true;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }

        //    #endregion

        //    #region Campagne

        //        public List<CsCentre> GetExploitationByUO(CsREFUNITEORGANISATIONNELLE UO)
        //        {
        //            try
        //            {
        //                using (var contex = new galadbEntities())
        //                {
        //                    List<CsCentre> centres = new List<CsCentre>();
        //                    foreach (var item in contex.CENTRE.ToList())
        //                    {
        //                        centres.Add(new CsCentre {  PK_ID=item.PK_ID,
        //                                                    CODECENTRE=item.CODECENTRE,
        //                                                    CODESITE=item.CODESITE,
        //                                                    DATECREATION=item.DATECREATION,
        //                                                    DATEMODIFICATION=item.DATEMODIFICATION,
        //                                                    FK_IDCODESITE=item.FK_IDCODESITE,
        //                                                    LIBELLE=item.LIBELLE,
        //                                                    LIBELLESITE=item.SITE.LIBELLE,
        //                                                    LIBELLETYPECENTRE=item.TYPECENTRE,
        //                                                    TYPECENTRE=item.TYPECENTRE,
        //                                                    USERCREATION=item.USERCREATION,
        //                                                    USERMODIFICATION=item.USERMODIFICATION
        //                        });
        //                    }
        //                    //return Entities.ConvertObject<CsCentre, CENTRE>(contex.CENTRE.ToList());
        //                    return centres;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        public bool InsertCampagneBTA(CsCampagnesBTAAccessiblesParLUO CampBAT)
        {

            try
            {
                using (var contexte = new galadbEntities())
                {
                    contexte.tbCampagnesControleBTA.Add(new tbCampagnesControleBTA
                    {
                        Campagne_ID = CampBAT.Campagne_ID,
                        CodeExploitation = CampBAT.CodeCentre,
                        //CODE = 0,
                        DateCreation = CampBAT.DateCreation,
                        DateDebutControles = CampBAT.DateDebutControles,
                        DateFinPrevue = CampBAT.DateFinPrevue,
                        DateModification = CampBAT.DateModification,
                        Libelle_Campagne = CampBAT.Libelle_Campagne,
                        MatriculeAgentCreation = CampBAT.MatriculeAgentCreation,
                        MatriculeAgentDerniereModification = CampBAT.MatriculeAgentDerniereModification,
                        NbreElements = CampBAT.NbreElements,
                        Statut_ID = CampBAT.Statut_ID,
                        FK_IDCENTRE = CampBAT.fk_idCentre
                        //ETATID=CampBAT.
                        
                    });
                    contexte.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCampagnesBTAAccessiblesParLUO> GetCampagneBTAControle()
        {
            try
            {
                using (var context = new galadbEntities())
                {

                    List<CsCampagnesBTAAccessiblesParLUO> ListeCampagne = Entities.ConvertObject<CsCampagnesBTAAccessiblesParLUO, tbCampagnesControleBTA>(context.tbCampagnesControleBTA.Where(c => c.Statut_ID==1).ToList());
                    var ele_pre = context.ElementsDeCampagneBTA.ToList();
                    var lots = context.tbLotsDeControleBTA.ToList();

                    if (ListeCampagne.Count() > 0)
                    {
                        foreach (var item in ListeCampagne)
                        {
                            ele_pre = ele_pre.Where(el_camp => el_camp.Campagne_ID == item.Campagne_ID).ToList();
                            lots = lots.Where(lot => lot.Campagne_ID == item.Campagne_ID && lot.StatutLot_ID==1).ToList();
                            
                            int NBREELEMENTS_PRESELECTION = ele_pre.Count();
                            item.CodeCentre = context.CENTRE.FirstOrDefault(c => c.PK_ID == item.fk_idCentre).CODE;
                            item.CodeUO = context.CENTRE.FirstOrDefault(c => c.PK_ID == item.fk_idCentre).CODESITE;
                            item.NbreElements = NBREELEMENTS_PRESELECTION;
                            item.ListElementsCamp = Entities.ConvertObject<CsElementsDeCampagneBTA, ElementsDeCampagneBTA>(ele_pre);
                            item.ListLot = Entities.ConvertObject<CstbLotsDeControleBTA, tbLotsDeControleBTA>(lots);
                            
                            foreach (var item_ in item.ListElementsCamp)
                            {

                               var obj= (from cl in context.CLIENT
                                       join ag in context.AG on new { cl.FK_IDAG } equals new { FK_IDAG = ag.PK_ID }
                                       join brt in context.BRT on new { FK_IDAG = ag.PK_ID } equals new { brt.FK_IDAG }
                                       where  item_.Contrat_ID==brt.PK_ID
                                       select cl).First();
                               item_.Nom = obj.NOMABON;
                               item_.Libelle_Centre = context.CENTRE.FirstOrDefault(c => c.PK_ID == obj.FK_IDCENTRE).LIBELLE;
                            }

                           
                            foreach (var item_ in item.ListLot)
                            {
                                var Elementlots = context.tbElementsLotDeControleBTA.Where(elmtlot => elmtlot.Lot_ID == item_.Lot_ID).ToList();
                                var Elmt = Entities.ConvertObject<CstbElementsLotDeControleBTA, tbElementsLotDeControleBTA>(Elementlots);
                                item_.ListElementLot=new List<CstbElementsLotDeControleBTA>();
                                item_.ListElementLot.AddRange(Elmt);
                            }


                        }
                        return ListeCampagne;
                    }
                    return new List<CsCampagnesBTAAccessiblesParLUO>();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //    #endregion



        public List<CsBrt> GetBranchementBTAControle(List<CsClient> ClientSelection)
        {

            try
            {
                using (var context = new galadbEntities())
                {
                    List<int> liste_ref_client = ClientSelection.Select(cl => cl.PK_ID ).ToList();
                    //var branchement = (from cl in context.CLIENT
                    //                   join ag in context.AG on new { cl.FK_IDAG } equals new { FK_IDAG = ag.PK_ID }
                    //                   join brt in context.BRT on new { FK_IDAG = ag.PK_ID } equals new { brt.FK_IDAG }
                    //                   where liste_ref_client.Contains(cl.PK_ID)
                    //                   select brt).ToList();

                    var obj = (from cl in context.CLIENT
                                       join ag in context.AG on new { cl.FK_IDAG } equals new { FK_IDAG = ag.PK_ID }
                                       join brt in context.BRT on new { FK_IDAG = ag.PK_ID } equals new { brt.FK_IDAG }
                                       join c in context.CENTRE on new { CENTRE=cl.FK_IDCENTRE } equals new {CENTRE= c.PK_ID }
                                       where liste_ref_client.Contains(cl.PK_ID)
                               select new CsBrt
                               {
                                   PK_ID = brt.PK_ID ,
                                   CENTRE =brt.CENTRE ,
                                   CLIENT = brt.CLIENT ,

                                   NOMABON = cl.NOMABON ,
                                   ADRESSERESEAU = brt.ADRESSERESEAU ,
                                   ANFAB = brt.ANFAB ,
                                   APPTRANSFO = brt.APPTRANSFO ,
                                   CATBRT = brt.CATBRT ,
                                   USERCREATION = brt.USERCREATION ,
                                   DATECREATION = brt.DATECREATION ,
                                   FK_IDPRODUIT = brt.FK_IDPRODUIT ,
                                   FK_IDCENTRE  = brt.FK_IDCENTRE , 


                                   //PK_ID = brt.PK_ID != null ? brt.PK_ID : int.MinValue,
                                   //CENTRE = c.LIBELLE!=string.Empty ? c.LIBELLE : string.Empty,
                                   //NOMABON = cl.NOMABON != string.Empty ? cl.NOMABON : string.Empty,
                                   //CLIENT = brt.CLIENT != string.Empty ? brt.CLIENT : string.Empty,
                                   //ADRESSERESEAU = brt.ADRESSERESEAU != string.Empty ? brt.ADRESSERESEAU : string.Empty,
                                   //ANFAB = brt.ANFAB != string.Empty ? brt.ANFAB : string.Empty,
                                   //APPTRANSFO = brt.APPTRANSFO != string.Empty ? brt.APPTRANSFO : string.Empty,
                                   //CATBRT = brt.CATBRT != string.Empty ? brt.CATBRT : string.Empty,
                                   //CODEDEPARTBT = brt.CODEBRT != string.Empty ? brt.CODEBRT : string.Empty,
                                   //CODEPOSTE = brt.CODEPOSTE != string.Empty ? brt.CODEPOSTE : string.Empty,

                                   //COEFPERTES = brt.COEFPERTES != null ? brt.COEFPERTES : decimal.MinValue,
                                   //DATECREATION = brt.DATECREATION != null ? brt.DATECREATION : System.DateTime.Now ,
                                   //DATEMODIFICATION = brt.DATEMODIFICATION != null ? brt.DATEMODIFICATION : System.DateTime.Now,
                                   //DMAJ = brt.DMAJ != null ? brt.DMAJ : System.DateTime.Now,
                                   //DRAC = brt.DRAC != null ? brt.DRAC : System.DateTime.Now,
                                   //DRES = brt.DRES != null ? brt.DRES : System.DateTime.Now,
                                   //FK_IDCENTRE = brt.FK_IDCENTRE != null ? brt.FK_IDCENTRE : int.MinValue,
                                   //FK_IDPRODUIT = brt.FK_IDPRODUIT != null ? brt.FK_IDPRODUIT : int.MinValue,
                                   //FK_IDTOURNEE = brt.AG.FK_IDTOURNEE != null ? brt.AG.FK_IDTOURNEE : int.MinValue,
                                   //LATITUDE = brt.LATITUDE != string.Empty ? brt.LATITUDE : string.Empty,
                                   //LONGBRT = brt.LONGBRT != null ? brt.LONGBRT : int.MinValue,
                                   //LONGITUDE = brt.LONGITUDE != string.Empty ? brt.LONGITUDE : string.Empty,
                                   //NATBRT = brt.NATBRT != string.Empty ? brt.NATBRT : string.Empty,
                                   //NBPOINT = brt.NBPOINT != null ? brt.NBPOINT : int.MinValue,
                                   //ORDTOUR = brt.AG.ORDTOUR != string.Empty ? brt.AG.ORDTOUR : string.Empty,
                                   //PERTES = brt.PERTES != null ? brt.PERTES : decimal.MinValue,
                                   //PUISSANCEINSTALLEE = brt.PUISSANCEINSTALLEE != null ? brt.PUISSANCEINSTALLEE : null ,
                                   //RESEAU = brt.RESEAU != string.Empty ? brt.RESEAU : string.Empty,
                                   //SERVICE = brt.SERVICE != string.Empty ? brt.SERVICE : string.Empty,
                                   //TRONCON = brt.TRONCON != string.Empty ? brt.TRONCON : string.Empty,
                                   //USERCREATION = brt.USERCREATION != string.Empty ? brt.USERCREATION : string.Empty,
                                   //USERMODIFICATION = brt.USERMODIFICATION != string.Empty ? brt.USERMODIFICATION : string.Empty
                               }).Distinct().ToList();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCategorieClient> GetTypeClient()
        {
            try
            {
                using (var context = new galadbEntities())
                {
                    List<CsCategorieClient> catclient = new List<CsCategorieClient>();
                    foreach (var item in context.CATEGORIECLIENT.ToList())
                    {
                        catclient.Add(new CsCategorieClient
                        {
                            DATECREATION = item.DATECREATION,
                            DATEMODIFICATION = item.DATEMODIFICATION,
                            LIBELLE = item.LIBELLE,
                            CODE = item.CODE,
                            PK_ID = item.PK_ID,
                            USERCREATION = item.USERCREATION,
                            USERMODIFICATION = item.USERMODIFICATION
                        });
                    }
                    return catclient;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsTypeTarif> GetTypeTarif()
        {
            try
            {
                using (var context = new galadbEntities())
                {
                    List<CsTypeTarif> tarifs = new List<CsTypeTarif>();
                    foreach (var item in context.TYPETARIF.ToList())
                    {
                        tarifs.Add(new CsTypeTarif
                        {
                            CODE = item.CODE,
                            DATECREATION = item.DATECREATION,
                            DATEMODIFICATION = item.DATECREATION,
                            FK_IDPRODUIT = item.FK_IDPRODUIT,
                            LIBELLE = item.LIBELLE,
                            PK_ID = item.PK_ID,
                            PRODUIT = item.PRODUIT,
                            USERCREATION = item.USERCREATION,
                            USERMODIFICATION = item.USERMODIFICATION
                        });
                    }
                    return tarifs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsGroupeDeFacturation> GetGroupeFacture()
        {
            try
            {
                List<CsGroupeDeFacturation> groupefacturation = new List<CsGroupeDeFacturation>();
                using (var context = new galadbEntities())
                {
                    foreach (var item in context.FREQUENCE)
                    {
                        groupefacturation.Add(new CsGroupeDeFacturation { GroupeDeFacturation = int.Parse(item.CODE), Libelle = item.LIBELLE });
                    }
                    return groupefacturation;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsReleveur> GetAgentZont()
        {
            try
            {
                using (var agents = new galadbEntities())
                {
                    var releveur = Entities.ConvertObject<CsReleveur, RELEVEUR>(agents.RELEVEUR.Distinct().ToList());

                    foreach (var item in releveur)
                    {
                        var Agent=agents.AGENT.FirstOrDefault(a=>a.MATRICULE==item.MATRICULE);
                        if (Agent!=null)
	                    {
                            item.NOMRELEVEUR = Agent.NOM + " " + Agent.PRENOM;
	                    }
                        item.NOMRELEVEUR = "( " + item.MATRICULE + " )" + item.NOMRELEVEUR;
                    }
                    return releveur;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        

        //#endregion


       // public List<List<CsProduit>> LoadProduit()
       // {
       //     return new List<List<CsProduit>>();
       // }

       // public bool DeleteBranchement(int brt_id)
       // {
       //     try
       //     {
       //         using (var context = new galadbEntities())
       //         {
       //             context.TBELEMENTPRESELECTIONCAMPAGNEBTA.Remove(context.TBELEMENTPRESELECTIONCAMPAGNEBTA.FirstOrDefault(md => md.BRT_ID == brt_id));
       //             context.SaveChanges();
       //             return true;
       //         }
       //     }
       //     catch (Exception ex)
       //     {
       //         throw ex;
       //     }
       // }

       // public List<CsEvenement> GetConsoInfo(string client,string periode,string nbr_cycle_fact)
       // {
       //     try
       //     {
       //         List<CsEvenement> liste_Historique = new List<CsEvenement>();
       //         using (var context = new galadbEntities())
       //         {
       //             //liste_Historique = Entities.ConvertObject<CsEvenement, EVENEMENT>(context.EVENEMENT.Take(100).ToList());

       //             liste_Historique =Entities.ConvertObject<CsEvenement, EVENEMENT>( context.EVENEMENT.Where(h => h.CLIENT == client 
       //                 //&& Convert.ToInt32(h.PERIODE) >= Convert.ToInt32(periode)
       //                                                             ).OrderByDescending(e => e.PERIODE)
       //                                                            .Select(_h => _h)
       //                                                            .OrderByDescending(p => p.PERIODE)
       //                                                            .Take(Convert.ToInt32(nbr_cycle_fact))
       //                                                            .ToList()); }
       //         return liste_Historique;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw ex;
       //     }
       // }

       // public CsClient GetClientInfo(string client)
       // {
       //     try
       //     {
       //         using (var context = new galadbEntities())
       //         {
       //             //return Entities.ConvertObject<CsClient, CLIENT>(context.CLIENT.FirstOrDefault(cl=>cl.REFCLIENT==client));
       //             var clientinfo = context.CLIENT.FirstOrDefault(cl => cl.REFCLIENT == client);
       //             CsClient leclient = new CsClient();
       //             leclient.REFCLIENT = clientinfo.REFCLIENT;
       //             leclient.NOMABON = clientinfo.NOMABON;
       //             return leclient;
       //         }
       //     }
       //     catch (Exception ex)
       //     {
       //         throw ex;
       //     }
       // }

       // public CsAbon GetContratInfo(string brt)
       // {
       //     try
       //     {
       //         using (var context = new galadbEntities())
       //         {
       //             var abon = (from client in context.CLIENT
       //                         from brt_ in client.BRT
       //                         from  ab in client.ABON 
       //                        select ab).ToList().Last();

       //             return new CsAbon
       //             {
       //                 AVANCE = abon.AVANCE
       //               ,
       //                 CENTRE = abon.CENTRE.CODECENTRE 
       //               //,
       //               //  CLIENT = abon.CLIENT
       //               ,
       //                 COEFFAC = abon.COEFFAC
       //               ,
       //                 CONSOMMATIONMAXI = abon.CONSOMMATIONMAXI
       //               ,
       //                 DABONNEMENT = abon.DABONNEMENT
       //               ,
       //                 DATECREATION = abon.DATECREATION
       //               ,
       //                 DATEMODIFICATION = abon.DATEMODIFICATION
       //               ,
       //                 DATERACBRT = abon.DATERACBRT
       //               ,
       //                 DAVANCE = abon.DAVANCE
       //               ,
       //                 DMAJ = abon.DMAJ
       //               ,
       //                 DRES = abon.DRES
       //               ,
       //                 FK_IDCENTRE = abon.FK_IDCENTRE
       //               ,
       //                 FK_IDCLIENT = abon.FK_IDCLIENT
       //               ,
       //                 FK_IDFORFAIT = abon.FK_IDFORFAIT
       //               ,
       //                 FK_IDMOISFAC = abon.FK_IDMOISFAC
       //               ,
       //                 FK_IDMOISREL = abon.FK_IDMOISREL
       //               ,
       //                 FK_IDNATURE = abon.FK_IDNATURE
       //               ,
       //                 FK_IDPRODUIT = abon.FK_IDPRODUIT
       //               ,
       //                 FK_IDTARIF = abon.FK_IDTARIF
       //               //,
       //               //  FORFAIT = abon.FORFAIT
       //               ,
       //                 FORFPERSO = abon.FORFPERSO
       //               //,
       //               //  MOISFAC = abon.MOISFAC
       //               //,
       //               //  MOISREL = abon.MOISREL
       //               ,
       //                 NATURE = abon.NATURE
       //               ,
       //                 NBFAC = abon.NBFAC
       //               //,
       //                 //ORDRE = abon.ORDRE
       //               ,
       //                 PERFAC = abon.PERFAC
       //               //,
       //               //  PERREL = abon.PERREL
       //               ,
       //                 PK_ID = abon.PK_ID
       //               //,
       //               //  PRODUIT = abon.PRODUIT
       //               //,
       //               //  PUISSANCE = abon.PUISSANCE
       //               ,
       //                 PUISSANCEUTILISEE = abon.PUISSANCEUTILISEE
       //               ,
       //                 RECU = abon.RECU
       //               ,
       //                 REGROU = abon.REGROU
       //               ,
       //                 RISTOURNE = abon.RISTOURNE
       //               //,
       //               //  TARIF = abon.TARIF
       //               ,
       //                 USERCREATION = abon.USERCREATION
       //               ,
       //                 USERMODIFICATION = abon.USERMODIFICATION
       //             };
       //         }
       //     }
       //     catch (Exception ex)
       //     {
       //         throw ex;
       //     }
       // }

       // public CsCanalisation GetCanalisationInfo(string brt)
       // {
       //     try
       //     {
       //         using (var context = new galadbEntities())
       //         {
       //             var brt_id = Convert.ToInt32(brt);
       //             return Entities.ConvertObject<CsCanalisation, CANALISATION>(context.CANALISATION.FirstOrDefault(cl => cl.BRT.PK_ID == brt_id));
       //         }
       //     }
       //     catch (Exception ex)
       //     {
       //         throw ex;
       //     }
       // }

        public List<CsTcompteur> GetTypeCompteur()
        {
            try
            {
                using (var context = new galadbEntities())
                {
                    List<CsTcompteur> tcomp = new List<CsTcompteur>();
                    foreach (var item in context.TYPECOMPTEUR.ToList())
                    {
                        tcomp.Add(new CsTcompteur
                        {
                            CODE = item.CODE,
                            DATECREATION = item.DATECREATION,
                            DATEMODIFICATION = item.DATECREATION,
                            FK_IDPRODUIT = item.FK_IDPRODUIT,
                            PK_ID = item.PK_ID,
                            PRODUIT = item.PRODUIT,
                            USERCREATION = item.USERCREATION,
                            USERMODIFICATION = item.USERMODIFICATION,
                            LIBELLE = item.LIBELLE
                        });
                    }
                    return tcomp;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int[] StatusEvenementFacture()
        {

                int[] strArray =
                {
                  Enumere.EvenementFacture  ,  Enumere.EvenementMisAJour  ,Enumere.EvenementPurger 
                };
                return strArray;

        }
        public List<CsClient> GetClientEligible(string CodeMethode, int FkidCentre, string CodeTypeClient, string CodeTypeTarif, string CodeTypeCompteur, string CodeAgentZone, string CodeGroupe, string nombremois, string txt_comparaison_periode1, string txt_comparaison_periode2, string txt_Code_Cas, double Nombre_Recurence, double pourcentage, string periode = "")
        {
            try
            {
                using (var context = new galadbEntities())
                {
                    var CLIENTS = context.CLIENT.Where(t => t.FK_IDCENTRE == FkidCentre).ToList();
                    int codemethode_ = int.Parse(CodeMethode);
                    var METHODEDETECTION = context.RefMethodesDeDetectionClientsBTA.FirstOrDefault(mth => mth.Methode_ID == codemethode_);
                    if (CodeTypeClient != "")
                    {
                        CLIENTS = CLIENTS.Where(cl => cl.CATEGORIE == CodeTypeClient).Select(c => c).Take(50).ToList();
                    }
                    if (CodeTypeTarif != "")
                    {
                        int codetypetarif_ = int.Parse(CodeTypeTarif);
                        CLIENTS = (from cl in CLIENTS
                                   from ab in cl.ABON
                                   where ab.FK_IDTYPETARIF == codetypetarif_
                                   select cl).ToList();

                    }
                    if (CodeTypeCompteur != "")
                    {
                        //int codeTypeCompteur_ = int.Parse(CodeTypeCompteur);
                        //CLIENTS = (from cl in CLIENTS
                        //           from brt_ in cl.BRT
                        //           where brt_.CANALISATION.Select(c => c.FK_IDTCOMPT).Contains(codeTypeCompteur_)
                        //           select cl).ToList();
                    }
                    if (CodeAgentZone != "")
                    {
                        //int codeagent=int.Parse(CodeAgentZone);
                        //CLIENTS = (from cl in CLIENTS
                        //          from a in cl.AG
                        //          where a.TOURNEE.FK_IDRELEVEUR == codeagent
                        //          select cl).ToList();
                    }
                    if (CodeGroupe != "")
                    {
                        CLIENTS = (from cl in CLIENTS
                                   from ab in cl.ABON
                                   where ab.PERFAC == CodeGroupe
                                   select cl).ToList();
                    }

                    //#region Sylla 08/07/2016

                    ////Si la méthode choisi est la methode de comparaison sur deux période
                    //if (methrech.Libele_Methode.Trim() == "Comparaison sur deux périodes".Trim())
                    //{
                    //    VisibiliteeElementMethodeComparaison(true);
                    //    VisibiliteeElementCritereParDefaut(false);
                    //    VisibiliteeElementMethodeRecurenceCas(false);
                    //}

                    ////Si la méthode choisi est la methode de comparaison sur plage de période
                    //if (methrech.Libele_Methode.Trim() == "Comparaison sur plage périodes".Trim())
                    //{
                    //    VisibiliteeElementMethodeComparaison(true);
                    //    VisibiliteeElementCritereParDefaut(false);
                    //    VisibiliteeElementMethodeRecurenceCas(false);

                    //}

                    ////Si la méthode choisi est la methode de Recurence des cas
                    //if (methrech.Libele_Methode.Trim() == "Recurence des cas".Trim())
                    //{
                    //    VisibiliteeElementMethodeComparaison(false);
                    //    VisibiliteeElementCritereParDefaut(false);
                    //    VisibiliteeElementMethodeRecurenceCas(true);

                    //}
                    //#endregion



                    List<CsClient> clients = new List<CsClient>();
                    const string MethodeDeChuteDeConso = "Méthode des chutes de Consommation";
                    const string IncohérencePuissanceSouscrite = "Incohérence Puissance Souscrite - Consommation";
                    const string Comparaisonsurdeuxpériodes = "Comparaison sur deux périodes";
                    const string Comparaisonsurplagepériodes = "Comparaison sur plage périodes";
                    const string Recurencedescas = "Recurence des cas";

                    int IDMethodeDeConso = METHODEDETECTION.Methode_ID;
                    string MethodeDeConso = METHODEDETECTION.Libele_Methode.Trim();
                    switch (MethodeDeConso)
                    {
                        case MethodeDeChuteDeConso:
                            clients = ChuteDeConsomation(nombremois, CLIENTS, METHODEDETECTION,pourcentage);
                            break;
                        case IncohérencePuissanceSouscrite:
                            string seuilDeConsomationSuperieurAPuissanceSouscrit = METHODEDETECTION.tbParametres.FirstOrDefault(param => param.LibelleParametre.Trim() == "Seuil de Consommation supérieur à la Puissance souscrite".Trim()).ValeurGlobaleInt.ToString();
                            clients = IncohérencePuissanceSouscriteConsommation(nombremois, seuilDeConsomationSuperieurAPuissanceSouscrit, CLIENTS.Select(c => c.REFCLIENT).ToList(), periode);
                            break;
                        case Comparaisonsurdeuxpériodes:
                            clients = ComparaisonSurDeuxPériodes(txt_comparaison_periode1, txt_comparaison_periode2, CLIENTS,pourcentage);
                            break;
                        case Comparaisonsurplagepériodes:
                            clients = ComparaisonSurPlagePériodes(txt_comparaison_periode1, txt_comparaison_periode2, CLIENTS,pourcentage);
                            break;
                        case Recurencedescas:
                            clients = RecurenceDesCas(nombremois, txt_Code_Cas, Nombre_Recurence, CLIENTS,METHODEDETECTION);
                            break;
                        default:
                            Console.WriteLine("Default case");
                            break;
                    }

                    return clients;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static List<CsClient> ChuteDeConsomation(string nombremois, List<CLIENT> CLIENTS, RefMethodesDeDetectionClientsBTA METHODEDETECTION, double pourcentage)
        {
            string leClient = string.Empty;
            try
            {
                List<CsClient> clients = new List<CsClient>();
                string nbrclyclefact = METHODEDETECTION.tbParametres.FirstOrDefault(param => param.LibelleParametre.Trim() == "Nombre de cycles facturation".Trim()).ValeurGlobaleInt.ToString();
                string pourcentachuteconsomax = METHODEDETECTION.tbParametres.FirstOrDefault(param => param.LibelleParametre.Trim() == "Pourcentage de chute de consommation".Trim()).ValeurGlobaleInt.ToString();
                foreach (var item in CLIENTS)
                {
                    leClient = item.REFCLIENT;
                    if (MéthodedeschutesdeConsommation(nbrclyclefact, pourcentage.ToString(), item.FK_IDCENTRE, item.REFCLIENT, item.ORDRE, nombremois))
                    {
                        CsClient obj = new CsClient();
                        obj.CENTRE = item.CENTRE != null ? item.CENTRE : "";
                        obj.DATECREATION = item.DATECREATION;
                        obj.DATEMODIFICATION = item.DATECREATION;
                        obj.FK_IDCENTRE = item.FK_IDCENTRE;
                        obj.PK_ID = item.PK_ID;
                        obj.USERCREATION = item.USERCREATION != null ? item.USERCREATION : "";
                        obj.USERMODIFICATION = item.USERMODIFICATION != null ? item.USERMODIFICATION : "";
                        obj.NOMABON = item.NOMABON != null ? item.NOMABON : "";
                        obj.REFCLIENT = item.REFCLIENT != null ? item.REFCLIENT : "";
                        obj.CODECONSO = item.CODECONSO != null ? item.CODECONSO : "";
                        obj.TELEPHONE = item.TELEPHONE != null ? item.TELEPHONE : "";
                        obj.CATEGORIE = !string.IsNullOrWhiteSpace(item.CATEGORIE) ? item.CATEGORIE : string.Empty;


                        clients.Add(obj);
                        if (clients.Count == 40) break;
                    }
                }
                return clients;
            }
            catch (Exception ex)
            {
                string leC = leClient;
                throw ex;
            }
        }
        public static bool MéthodedeschutesdeConsommation(string Nombredecyclesfacturation, string PourcentagedechutedeconsommationMax, int fk_idcentre, string refclient, string ordre, string periodedepart)
        {
            try
            {
                List<HISTORIQUE> LISTE_CONSO_EVENEMENT = new List<HISTORIQUE>();
                List<int?> ListeChuteConsoEnPourcentage = new List<int?>();
                int? Pourcentagedechutedeconsommation = 0;
                bool EstEligible = false;
                int periode_dep;
                Int16 nbr_cycle;
                using (var context = new galadbEntities())
                {
                    periode_dep = Convert.ToInt32(periodedepart);
                    nbr_cycle = Convert.ToInt16(Nombredecyclesfacturation);
                    LISTE_CONSO_EVENEMENT = context.HISTORIQUE.Where(ev => ev.FK_IDCENTRE == fk_idcentre && ev.CLIENT == refclient && ev.ORDRE == ordre).AsEnumerable().Where(ev_ => ConvertAndCompar(periode_dep, ev_.PERIODE) == true)
                                                                    .OrderBy(ev__ => ev__.PERIODE).Take(nbr_cycle).ToList();
                }
                var CONSO_EVENEMENT = LISTE_CONSO_EVENEMENT;

                var liste_conso_client = CONSO_EVENEMENT.GroupBy(p => new { p.CLIENT, p.PERIODE }, p => p.CONSO,
                         (key, g) => new { MyKey = key, Conso = g.Sum() }).Select(c => c.Conso).ToList();


                for (int i = 0; i < liste_conso_client.Count(); i++)
                {
                    if (i < liste_conso_client.Count() - 1)
                    {
                        if (liste_conso_client[i] > 0)
                        {
                            ListeChuteConsoEnPourcentage.Add(100 - ((liste_conso_client[i + 1] * 100) / liste_conso_client[i]));
                        }
                    }
                }

                if (ListeChuteConsoEnPourcentage.Count() > 0)
                {
                    Pourcentagedechutedeconsommation = ListeChuteConsoEnPourcentage.Sum() / ListeChuteConsoEnPourcentage.Count();
                    EstEligible = Pourcentagedechutedeconsommation > int.Parse(PourcentagedechutedeconsommationMax) ? true : false;
                    return EstEligible;
                }
                else
                {
                    return EstEligible;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<CsClient> ComparaisonSurDeuxPériodes(string txt_comparaison_periode1, string txt_comparaison_periode2, List<CLIENT> CLIENTS,double pourcentage)
        {
            List<CsClient> clients = new List<CsClient>();

            foreach (var item in CLIENTS)
            {
                if (EligilbleAComparaisonSurDeuxPériodes(item, txt_comparaison_periode1, txt_comparaison_periode2,pourcentage))
                {
                    CsClient obj = new CsClient();
                    obj.CENTRE = item.CENTRE != null ? item.CENTRE : "";
                    obj.DATECREATION = item.DATECREATION;
                    obj.DATEMODIFICATION = item.DATECREATION;
                    obj.FK_IDCENTRE = item.FK_IDCENTRE;
                    obj.PK_ID = item.PK_ID;
                    obj.USERCREATION = item.USERCREATION != null ? item.USERCREATION : "";
                    obj.USERMODIFICATION = item.USERMODIFICATION != null ? item.USERMODIFICATION : "";
                    obj.NOMABON = item.NOMABON != null ? item.NOMABON : "";
                    obj.REFCLIENT = item.REFCLIENT != null ? item.REFCLIENT : "";
                    obj.CODECONSO = item.CODECONSO != null ? item.CODECONSO : "";
                    obj.TELEPHONE = item.TELEPHONE != null ? item.TELEPHONE : "";
                    obj.CATEGORIE = !string.IsNullOrWhiteSpace(item.CATEGORIE) ? item.CATEGORIE : string.Empty;


                    clients.Add(obj);
                    if (clients.Count ==10) break;

                }
            }
            return clients;
        }
        private static bool EligilbleAComparaisonSurDeuxPériodes(CLIENT LeClient, string txt_comparaison_periode1, string txt_comparaison_periode2, double pourcentage)
        {
            using (var context = new galadbEntities())
            {
                List<int?> ListeChuteConsoEnPourcentage = new List<int?>();
                int? Pourcentagedechutedeconsommation = 0;
                bool EstEligible = false;
                var LISTE_CONSO_EVENEMENT_PERIODE1 = context.HISTORIQUE.Where(ev => ev.FK_IDCENTRE == LeClient.FK_IDCENTRE && ev.CLIENT == LeClient.REFCLIENT && ev.ORDRE == LeClient.ORDRE && ev.PERIODE == txt_comparaison_periode1).ToList();
                var LISTE_CONSO_EVENEMENT_PERIODE2 = context.HISTORIQUE.Where(ev => ev.FK_IDCENTRE == LeClient.FK_IDCENTRE && ev.CLIENT == LeClient.REFCLIENT && ev.ORDRE == LeClient.ORDRE && ev.PERIODE == txt_comparaison_periode2).ToList();




                LISTE_CONSO_EVENEMENT_PERIODE1.AddRange(LISTE_CONSO_EVENEMENT_PERIODE2) ;
                var CONSO_EVENEMENT = LISTE_CONSO_EVENEMENT_PERIODE1;
                var liste_conso_client = CONSO_EVENEMENT.GroupBy(p => new { p.CLIENT, p.PERIODE }, p => p.CONSO,
                         (key, g) => new { MyKey = key, Conso = g.Sum() }).Select(c => c.Conso).ToList();


                for (int i = 0; i < liste_conso_client.Count(); i++)
                {
                    if (i < liste_conso_client.Count() - 1)
                    {
                        if (liste_conso_client[i] > 0)
                        {
                            ListeChuteConsoEnPourcentage.Add(100 - ((liste_conso_client[i + 1] * 100) / liste_conso_client[i]));
                        }
                    }
                }

                if (ListeChuteConsoEnPourcentage.Count() > 0)
                {
                    Pourcentagedechutedeconsommation = ListeChuteConsoEnPourcentage.Sum() / ListeChuteConsoEnPourcentage.Count();
                    EstEligible = Pourcentagedechutedeconsommation > int.Parse(pourcentage.ToString()) ? true : false;
                    return EstEligible;
                }
                else
                {
                    return EstEligible;
                }
                
                
                //foreach (var item in LISTE_CONSO_EVENEMENT_PERIODE1)
                //{
                //    EstEligible = LISTE_CONSO_EVENEMENT_PERIODE2.Select(c => c.CONSO).Contains(item.CONSO);
                //    if (EstEligible == true)
                //    {
                //        break;
                //    }
                //}
                //return EstEligible;
            }
        }
        private List<CsClient> ComparaisonSurPlagePériodes(string txt_comparaison_periode1, string txt_comparaison_periode2, List<CLIENT> CLIENTS, double pourcentage)
        {
            List<CsClient> clients = new List<CsClient>();

            foreach (var item in CLIENTS)
            {
                if (EligilbleAComparaisonSurPlagePériodes(item, txt_comparaison_periode1, txt_comparaison_periode2, pourcentage))
                {
                    CsClient obj = new CsClient();
                    obj.CENTRE = item.CENTRE != null ? item.CENTRE : "";
                    obj.DATECREATION = item.DATECREATION;
                    obj.DATEMODIFICATION = item.DATECREATION;
                    obj.FK_IDCENTRE = item.FK_IDCENTRE;
                    obj.PK_ID = item.PK_ID;
                    obj.USERCREATION = item.USERCREATION != null ? item.USERCREATION : "";
                    obj.USERMODIFICATION = item.USERMODIFICATION != null ? item.USERMODIFICATION : "";
                    obj.NOMABON = item.NOMABON != null ? item.NOMABON : "";
                    obj.REFCLIENT = item.REFCLIENT != null ? item.REFCLIENT : "";
                    obj.CODECONSO = item.CODECONSO != null ? item.CODECONSO : "";
                    obj.TELEPHONE = item.TELEPHONE != null ? item.TELEPHONE : "";
                    obj.CATEGORIE = !string.IsNullOrWhiteSpace(item.CATEGORIE) ? item.CATEGORIE : string.Empty;


                    clients.Add(obj);
                    if (clients.Count == 10) break;

                }
            }
            return clients;
        }

        private bool EligilbleAComparaisonSurPlagePériodes(CLIENT LeClient, string txt_comparaison_periode1, string txt_comparaison_periode2, double pourcentage)
        {
            using (var context = new galadbEntities())
            {
                List<int?> ListeChuteConsoEnPourcentage = new List<int?>();
                int? Pourcentagedechutedeconsommation = 0;
                int txt_comparaison_periode1_=int.Parse(txt_comparaison_periode1);
                int txt_comparaison_periode2_=int.Parse(txt_comparaison_periode2);
                bool EstEligible = false;
                var LISTE_CONSO_EVENEMENT_PERIODE1 = context.HISTORIQUE.Where(ev => ev.FK_IDCENTRE == LeClient.FK_IDCENTRE && ev.CLIENT == LeClient.REFCLIENT && ev.ORDRE == LeClient.ORDRE ).AsEnumerable().Where(ev_ =>  ConvertAndCompar(txt_comparaison_periode1_,ev_.PERIODE) && ConvertAndComparInverse(txt_comparaison_periode2_,ev_.PERIODE)).ToList();

                var CONSO_EVENEMENT = LISTE_CONSO_EVENEMENT_PERIODE1;
                var liste_conso_client = CONSO_EVENEMENT.GroupBy(p => new { p.CLIENT, p.PERIODE }, p => p.CONSO,
                         (key, g) => new { MyKey = key, Conso = g.Sum() }).Select(c => c.Conso).ToList();


                for (int i = 0; i < liste_conso_client.Count(); i++)
                {
                    if (i < liste_conso_client.Count() - 1)
                    {
                        if (liste_conso_client[i] > 0)
                        {
                            ListeChuteConsoEnPourcentage.Add(100 - ((liste_conso_client[i + 1] * 100) / liste_conso_client[i]));
                        }
                    }
                }

                if (ListeChuteConsoEnPourcentage.Count() > 0)
                {
                    Pourcentagedechutedeconsommation = ListeChuteConsoEnPourcentage.Sum() / ListeChuteConsoEnPourcentage.Count();
                    EstEligible = Pourcentagedechutedeconsommation > int.Parse(pourcentage.ToString()) ? true : false;
                    return EstEligible;
                }
                else
                {
                    return EstEligible;
                }


            }
        }
        private List<CsClient> RecurenceDesCas( string nombremois, string txt_Code_Cas, double Nombre_Recurence, List<CLIENT> CLIENTS, RefMethodesDeDetectionClientsBTA METHODEDETECTION)
        {
            List<CsClient> clients = new List<CsClient>();
            string nbrclyclefact = METHODEDETECTION.tbParametres.FirstOrDefault(param => param.LibelleParametre.Trim() == "Nombre de cycles facturation".Trim()).ValeurGlobaleInt.ToString();

            foreach (var item in CLIENTS)
            {
                if (EligilbleARecurenceDesCas(nbrclyclefact,nombremois, item, txt_Code_Cas, Nombre_Recurence))
                {
                    CsClient obj = new CsClient();
                    obj.CENTRE = item.CENTRE != null ? item.CENTRE : "";
                    obj.DATECREATION = item.DATECREATION;
                    obj.DATEMODIFICATION = item.DATECREATION;
                    obj.FK_IDCENTRE = item.FK_IDCENTRE;
                    obj.PK_ID = item.PK_ID;
                    obj.USERCREATION = item.USERCREATION != null ? item.USERCREATION : "";
                    obj.USERMODIFICATION = item.USERMODIFICATION != null ? item.USERMODIFICATION : "";
                    obj.NOMABON = item.NOMABON != null ? item.NOMABON : "";
                    obj.REFCLIENT = item.REFCLIENT != null ? item.REFCLIENT : "";
                    obj.CODECONSO = item.CODECONSO != null ? item.CODECONSO : "";
                    obj.TELEPHONE = item.TELEPHONE != null ? item.TELEPHONE : "";
                    obj.CATEGORIE = !string.IsNullOrWhiteSpace(item.CATEGORIE) ? item.CATEGORIE : string.Empty;


                    clients.Add(obj);
                    if (clients.Count == 10) break;

                }
            }
            return clients;
        }
        private bool EligilbleARecurenceDesCas(string nbr_cycle, string periodedeb, CLIENT LeClient, string txt_Code_Cas, double Nombre_Recurence)
        {
            using (var context = new galadbEntities())
            {
                bool EstEligible = false;
                int Nombre_Occurence = 0;
                int period_Debut = int.Parse(periodedeb);
                int nbr_cycle_ = int.Parse(nbr_cycle);
                var LISTE_CONSO_EVENEMENT_PERIODE = context.HISTORIQUE.Where(ev => ev.FK_IDCENTRE == LeClient.FK_IDCENTRE && ev.CLIENT == LeClient.REFCLIENT && ev.ORDRE == LeClient.ORDRE).AsEnumerable().Where(ev_ => ConvertAndCompar(period_Debut, ev_.PERIODE) == true)
                                                                                    .OrderBy(ev__ => ev__.PERIODE).Take(nbr_cycle_).ToList();
                foreach (var item in LISTE_CONSO_EVENEMENT_PERIODE)
                {
                    var ContienCas = item.CAS == txt_Code_Cas ? true : false;
                    if (ContienCas == true)
                    {
                        Nombre_Occurence++;
                    }
                }
                if (Nombre_Occurence >= Nombre_Recurence)
                {
                    EstEligible = true;
                }
                return EstEligible;
            }
        }


        private static bool ConvertAndCompar(int periode_dep, string ev)
        {
            if (!string.IsNullOrWhiteSpace(ev))
            {
                return Convert.ToInt32(ev) >= periode_dep;
            }
            return false;
        }
        private static bool ConvertAndComparInverse(int periode_dep, string ev)
        {
            if (!string.IsNullOrWhiteSpace(ev))
            {
                return Convert.ToInt32(ev) <= periode_dep;
            }
            return false;
        }
        public static List<CsClient> IncohérencePuissanceSouscriteConsommation(string Nombredecyclesfacturation, string SeuildeConsommationSupérieurAPuissancesouscrite, List<string> refclient, string periodedepart)
        {
            try
            {
                return new List<CsClient>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetMois()
        {
            try
            {
                using (var context = new galadbEntities())
                {
                    List<string> liste_ev = new List<string>();
                    var event_context = context.EVENEMENT.Select(ev => ev.PERIODE).Distinct().ToList().OrderBy(ev_ => ev_);
                    foreach (var item in event_context)
                    {
                        liste_ev.Add(item);
                    }
                    return liste_ev;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       //public List<TBELEMENTLOTDECONTROLEBTA> ELEMENTLOTDECONTROLEBTA(CsTBLOTDECONTROLEBTA CampBAT)
       //{
       //    try
       //    {
       //        using (var context = new galadbEntities())
       //        {
       //            return context.TBELEMENTLOTDECONTROLEBTA.Where(elmt => elmt.LOT_ID == CampBAT.LOT_ID).ToList();
       //        }
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}

       //public List<TBLOTDECONTROLEBTA> LOTDECONTROLEBTA(CsTBCAMPAGNECONTROLEBTA CampBAT)
       //{
       //    try
       //    {
       //        using (var context = new galadbEntities())
       //        {
       //            return context.TBLOTDECONTROLEBTA.Where(elmt => elmt.CAMPAGNE_ID == CampBAT.CAMPAGNE_ID).ToList();
       //        }
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}

        //public List<tbElementPreselectionCampagnesBTA> ELEMENTPRESELECTIONCAMPAGNEBTA(CsTBCAMPAGNE CampBAT)
        //{
        //    try
        //    {
        //        using (var context = new galadbEntities())
        //        {
        //            return context.tbElementPreselectionCampagnesBTA.Where(elmt => elmt.ACTIF == true && elmt.CAMPAGNE_ID == CampBAT.CAMPAGNE_ID).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool SaveCampagneElement(List<CsCampagnesBTAAccessiblesParLUO> ListeCampBAT)
        {
            try
            {
                return SaveElementNonAffecteALot(ListeCampBAT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SaveElementNonAffecteALot(List<CsCampagnesBTAAccessiblesParLUO> ListeCampBAT)
        {
            List<tbCampagnesControleBTA> Camps = new List<tbCampagnesControleBTA>();

            List<ElementsDeCampagneBTA> TBELEMENTPRESELECTIONCAMPAGNEBTANewList = new List<ElementsDeCampagneBTA>();
            List<ElementsDeCampagneBTA> TBELEMENTPRESELECTIONCAMPAGNEBTA_Delete = new List<ElementsDeCampagneBTA>();

            List<tbLotsDeControleBTA> TBLOTDECONTROLEBTANewList = new List<tbLotsDeControleBTA>();
            List<tbLotsDeControleBTA> TBLOTDECONTROLEBTA_Delete = new List<tbLotsDeControleBTA>();

            List<tbElementsLotDeControleBTA> TBELEMENTLOTDECONTROLEBTANewList = new List<tbElementsLotDeControleBTA>();
            List<tbElementsLotDeControleBTA> TBELEMENTLOTDECONTROLEBTA_Delete = new List<tbElementsLotDeControleBTA>();



         

            using (galadbEntities context = new galadbEntities())
            {
                foreach (var CampBAT in ListeCampBAT)
                {
                   
                    TBELEMENTPRESELECTIONCAMPAGNEBTANewList.AddRange(Entities.ConvertObject<ElementsDeCampagneBTA, CsElementsDeCampagneBTA>(CampBAT.ListElementsCamp));
                    TBELEMENTPRESELECTIONCAMPAGNEBTA_Delete.AddRange(context.ElementsDeCampagneBTA.Where(c => c.Campagne_ID == CampBAT.Campagne_ID));

                    CampBAT.ListLot.ForEach(c => c.Lot_ID = Guid.NewGuid());

                    TBLOTDECONTROLEBTANewList.AddRange(Entities.ConvertObject<tbLotsDeControleBTA, CstbLotsDeControleBTA>(CampBAT.ListLot));
                    TBLOTDECONTROLEBTA_Delete.AddRange(context.tbLotsDeControleBTA.Where(c => c.Campagne_ID == CampBAT.Campagne_ID));
                    TBLOTDECONTROLEBTA_Delete.ForEach(c => c.StatutLot_ID = 0);
                    foreach (var item in CampBAT.ListLot)
                    {
                        item.ListElementLot.ForEach(c => c.Lot_ID = item.Lot_ID);
                        TBELEMENTLOTDECONTROLEBTANewList.AddRange(Entities.ConvertObject<tbElementsLotDeControleBTA, CstbElementsLotDeControleBTA>(item.ListElementLot));
                        //TBELEMENTLOTDECONTROLEBTA_Delete.AddRange(context.tbElementsLotDeControleBTA.Where(c => c.Lot_ID == item.Lot_ID));
                    }
                    //foreach (var item in context.tbLotsDeControleBTA.Where(c => c.Campagne_ID == CampBAT.Campagne_ID))
                    //{
                    //    TBELEMENTLOTDECONTROLEBTA_Delete.AddRange(context.tbElementsLotDeControleBTA.Where(c => c.Lot_ID == item.Lot_ID));
                    //}
                }

                if (TBELEMENTPRESELECTIONCAMPAGNEBTANewList.Count() > 0)
                    Entities.InsertEntity<ElementsDeCampagneBTA>(TBELEMENTPRESELECTIONCAMPAGNEBTANewList, context);
                if (TBELEMENTPRESELECTIONCAMPAGNEBTA_Delete.Count() > 0)
                    Entities.DeleteEntity<ElementsDeCampagneBTA>(TBELEMENTPRESELECTIONCAMPAGNEBTA_Delete, context);

                if (TBLOTDECONTROLEBTANewList.Count() > 0)                
                    Entities.InsertEntity<tbLotsDeControleBTA>(TBLOTDECONTROLEBTANewList, context);
                
                if (TBLOTDECONTROLEBTA_Delete.Count() > 0)
                    Entities.UpdateEntity<tbLotsDeControleBTA>(TBLOTDECONTROLEBTA_Delete, context);

                if (TBELEMENTLOTDECONTROLEBTANewList.Count() > 0)
                    Entities.InsertEntity<tbElementsLotDeControleBTA>(TBELEMENTLOTDECONTROLEBTANewList, context);
                //if (TBELEMENTLOTDECONTROLEBTA_Delete.Count() > 0)
                //    Entities.DeleteEntity<tbElementsLotDeControleBTA>(TBELEMENTLOTDECONTROLEBTA_Delete, context);


                context.SaveChanges();

                foreach (var Campagne in context.tbCampagnesControleBTA)
                {
                    if (context.ElementsDeCampagneBTA.Where(c => c.Campagne_ID == Campagne.Campagne_ID).Count() <= 0)
                    {
                        
                        context.tbCampagnesControleBTA.FirstOrDefault(c => c.Campagne_ID == Campagne.Campagne_ID).Statut_ID =  0;
                    }
                    else
                    {
                        context.tbCampagnesControleBTA.FirstOrDefault(c => c.Campagne_ID == Campagne.Campagne_ID).Statut_ID = 1;
                    }
                }

                context.SaveChanges();

                return true;
            }

        }

       //private void Handle_Lot(CsREFMETHODEDEDETECTIONCLIENTSBTA Mth_Detection, List<TBLOTDECONTROLEBTA> TBLOTDECONTROLEBTA_Update, List<TBLOTDECONTROLEBTA> TBLOTDECONTROLEBTANewList, List<TBLOTDECONTROLEBTA> TBLOTDECONTROLEBTA_Delete, List<TBELEMENTLOTDECONTROLEBTA> TBELEMENTLOTDECONTROLEBTA_Update, List<TBELEMENTLOTDECONTROLEBTA> TBELEMENTLOTDECONTROLEBTANewList, List<TBELEMENTLOTDECONTROLEBTA> TBELEMENTLOTDECONTROLEBTA_Delete, CsTBCAMPAGNECONTROLEBTA CampBAT)
       //{
       //    foreach (var LOT in CampBAT.LISTELOT)
       //    {
       //        TBELEMENTLOTDECONTROLEBTA_Delete.AddRange(ELEMENTLOTDECONTROLEBTA(LOT));

       //        Handle_ElementLot(Mth_Detection, TBELEMENTLOTDECONTROLEBTA_Update, TBELEMENTLOTDECONTROLEBTANewList, TBELEMENTLOTDECONTROLEBTA_Delete, LOT);


       //        if (TBLOTDECONTROLEBTA_Delete.FirstOrDefault(el => el.LOT_ID == LOT.LOT_ID) == null)
       //        {
       //            //campagne = Entities.ConvertObject<TBCAMPAGNECONTROLEBTA, CsTBCAMPAGNECONTROLEBTA>(_item);
       //            TBLOTDECONTROLEBTANewList.Add(new TBLOTDECONTROLEBTA
       //            {
       //                CAMPAGNE_ID = LOT.CAMPAGNE_ID,
       //                DATECREATION = DateTime.Now,
       //                CRITERE_GROUPEDEFACTURATION = LOT.CRITERE_GROUPEDEFACTURATION,
       //                CRITERE_IDTOURNEE = LOT.CRITERE_IURNEE,
       //                CRITERE_TYPECLIENT = LOT.CRITERE_TYPECLIENT,
       //                CRITERE_TYPECOMPTEUR = LOT.CRITERE_TYPECOMPTEUR,
       //                CRITERE_TYPETARIF = LOT.CRITERE_TYPETARIF,
       //                DATEFERMETURE = LOT.DATEFERMETURE,
       //                LIBELLE_LOT = LOT.LIBELLE_LOT,
       //                LOT_ID = LOT.LOT_ID,
       //                MATRICULEAGENTCONTROLEUR = LOT.MATRICULEAGENTCONTROLEUR,
       //                MATRICULEAGENTCREATION = LOT.MATRICULEAGENTCREATION,
       //                NBREELEMENTSDULOT = LOT.NBREELEMENTSDULOT,
       //                STATUTLOT_ID = LOT.STATUTLOT_ID

       //            });
       //        }
       //        else
       //        {
       //            TBLOTDECONTROLEBTA lot = TBLOTDECONTROLEBTA_Delete.FirstOrDefault(el => el.LOT_ID == LOT.LOT_ID);

       //            lot.CAMPAGNE_ID = LOT.CAMPAGNE_ID;
       //            lot.DATECREATION = DateTime.Now;
       //            lot.CRITERE_GROUPEDEFACTURATION = LOT.CRITERE_GROUPEDEFACTURATION;
       //            lot.CRITERE_IDTOURNEE = LOT.CRITERE_IURNEE;
       //            lot.CRITERE_TYPECLIENT = LOT.CRITERE_TYPECLIENT;
       //            lot.CRITERE_TYPECOMPTEUR = LOT.CRITERE_TYPECOMPTEUR;
       //            lot.CRITERE_TYPETARIF = LOT.CRITERE_TYPETARIF;
       //            lot.DATEFERMETURE = LOT.DATEFERMETURE;
       //            lot.LIBELLE_LOT = LOT.LIBELLE_LOT;
       //            lot.LOT_ID = LOT.LOT_ID;
       //            lot.MATRICULEAGENTCONTROLEUR = LOT.MATRICULEAGENTCONTROLEUR;
       //            lot.MATRICULEAGENTCREATION = LOT.MATRICULEAGENTCREATION;
       //            lot.NBREELEMENTSDULOT = LOT.NBREELEMENTSDULOT;
       //            lot.STATUTLOT_ID = LOT.STATUTLOT_ID;

       //            TBLOTDECONTROLEBTA_Update.Add(lot);
       //            TBLOTDECONTROLEBTA_Delete.Remove(lot);
       //        }
       //    }
       //}

       //private static void Handle_ElementLot(CsREFMETHODEDEDETECTIONCLIENTSBTA Mth_Detection, List<TBELEMENTLOTDECONTROLEBTA> TBELEMENTLOTDECONTROLEBTA_Update, List<TBELEMENTLOTDECONTROLEBTA> TBELEMENTLOTDECONTROLEBTANewList, List<TBELEMENTLOTDECONTROLEBTA> TBELEMENTLOTDECONTROLEBTA_Delete, CsTBLOTDECONTROLEBTA LOT)
       //{
       //    foreach (var ELEMENTLOT in LOT.TBELEMENTLOTDECONTROLEBTA)
       //    {
       //        if (TBELEMENTLOTDECONTROLEBTA_Delete.FirstOrDefault(el => el.BRT_ID == ELEMENTLOT.BRT_ID) == null)
       //        {
       //         TBELEMENTLOTDECONTROLEBTA obj=   new TBELEMENTLOTDECONTROLEBTA();
                   
       //                obj.BRT_ID =ELEMENTLOT.BRT_ID!=null? ELEMENTLOT.BRT_ID:int.MinValue;
       //                if (LOT.LOT_ID!=null)
       //                 {
       //                     obj.LOT_ID = LOT.LOT_ID;
       //                 }
       //                obj.DATESELECTION = DateTime.Now;
       //                obj.DEBUT_PERAA = LOT.DATECREATION != null ? LOT.DATECREATION.Year : 00;
       //                obj.DEBUT_PERMM = LOT.DATECREATION != null ? LOT.DATECREATION.Month : 00;
       //                obj.FIN_PERAA = LOT.DATEFERMETURE != null ? LOT.DATEFERMETURE.Value.Year : 00;
       //                obj.FIN_PERMM = LOT.DATEFERMETURE != null ? LOT.DATEFERMETURE.Value.Month : 00;
       //                obj.METHODE_ID = Mth_Detection.METHODE_ID!=null? Mth_Detection.METHODE_ID.Value:int.MinValue;
       //                obj.NONABON = ELEMENTLOT.NOMABON;
       //                TBELEMENTLOTDECONTROLEBTANewList.Add(obj);
       //        }
       //        else
       //        {
       //            TBELEMENTLOTDECONTROLEBTA ELEMENTLOTDECONTROLEBTA_ = TBELEMENTLOTDECONTROLEBTA_Delete.FirstOrDefault(el => el.BRT_ID == ELEMENTLOT.BRT_ID);
       //            ELEMENTLOTDECONTROLEBTA_.DATESELECTION = DateTime.Now;
       //            ELEMENTLOTDECONTROLEBTA_.DEBUT_PERAA = LOT.DATECREATION != null ? LOT.DATECREATION.Year : 00;
       //            ELEMENTLOTDECONTROLEBTA_.DEBUT_PERMM = LOT.DATECREATION != null ? LOT.DATECREATION.Month : 00;
       //            ELEMENTLOTDECONTROLEBTA_.FIN_PERAA = LOT.DATEFERMETURE != null ? LOT.DATEFERMETURE.Value.Year : 00;
       //            ELEMENTLOTDECONTROLEBTA_.FIN_PERMM = LOT.DATEFERMETURE != null ? LOT.DATEFERMETURE.Value.Month : 00;
       //            ELEMENTLOTDECONTROLEBTA_.METHODE_ID = Mth_Detection.METHODE_ID != null ? Mth_Detection.METHODE_ID.Value : int.MinValue;
       //            //ELEMENTLOTDECONTROLEBTA_.ACTIF = true;
       //            ELEMENTLOTDECONTROLEBTA_.NONABON = ELEMENTLOT.NOMABON;
       //            TBELEMENTLOTDECONTROLEBTA_Update.Add(ELEMENTLOTDECONTROLEBTA_);
       //            TBELEMENTLOTDECONTROLEBTA_Delete.Remove(ELEMENTLOTDECONTROLEBTA_);
       //        }
       //    }
       //}

       //private static void Handle_ElementPreselection(CsREFMETHODEDEDETECTIONCLIENTSBTA Mth_Detection, List<TBELEMENTPRESELECTIONCAMPAGNEBTA> TBELEMENTPRESELECTIONCAMPAGNEBTA_Update, List<TBELEMENTPRESELECTIONCAMPAGNEBTA> TBELEMENTPRESELECTIONCAMPAGNEBTANewList, List<TBELEMENTPRESELECTIONCAMPAGNEBTA> TBELEMENTPRESELECTIONCAMPAGNEBTA_Delete, CsTBCAMPAGNECONTROLEBTA CampBAT)
       //{
       //    foreach (var ELEMENTPRESELECTION in CampBAT.LISTEBRANCHEMENT)
       //    {
       //        if (TBELEMENTPRESELECTIONCAMPAGNEBTA_Delete.FirstOrDefault(el => el.BRT_ID == ELEMENTPRESELECTION.PK_ID) == null)
       //        {
       //            //campagne = Entities.ConvertObject<TBCAMPAGNECONTROLEBTA, CsTBCAMPAGNECONTROLEBTA>(_item);
       //            TBELEMENTPRESELECTIONCAMPAGNEBTANewList.Add(new TBELEMENTPRESELECTIONCAMPAGNEBTA
       //            {
       //                BRT_ID = ELEMENTPRESELECTION.PK_ID,
       //                CAMPAGNE_ID = CampBAT.CAMPAGNE_ID,
       //                DATESELECTION = DateTime.Now,
       //                DEBUT_PERAA = CampBAT.DATEDEBUTCONTROLES.Year,
       //                DEBUT_PERMM = CampBAT.DATEDEBUTCONTROLES.Month,
       //                FIN_PERAA = CampBAT.DATEFINPREVUE.Year,
       //                FIN_PERMM = CampBAT.DATEFINPREVUE.Month,
       //                METHODE_ID = Mth_Detection.METHODE_ID.Value,
       //                ACTIF = true
       //            });
       //        }
       //        else
       //        {
       //            TBELEMENTPRESELECTIONCAMPAGNEBTA ELEMENTPRESELECTION_ = TBELEMENTPRESELECTIONCAMPAGNEBTA_Delete.FirstOrDefault(el => el.BRT_ID == ELEMENTPRESELECTION.PK_ID);
       //            ELEMENTPRESELECTION_.DATESELECTION = DateTime.Now;
       //            ELEMENTPRESELECTION_.DEBUT_PERAA = CampBAT.DATEDEBUTCONTROLES.Year;
       //            ELEMENTPRESELECTION_.DEBUT_PERMM = CampBAT.DATEDEBUTCONTROLES.Month;
       //            ELEMENTPRESELECTION_.FIN_PERAA = CampBAT.DATEFINPREVUE.Year;
       //            ELEMENTPRESELECTION_.FIN_PERMM = CampBAT.DATEFINPREVUE.Month;
       //            ELEMENTPRESELECTION_.METHODE_ID = Mth_Detection.METHODE_ID.Value;
       //            ELEMENTPRESELECTION_.ACTIF = true;
       //            TBELEMENTPRESELECTIONCAMPAGNEBTA_Update.Add(ELEMENTPRESELECTION_);
       //            TBELEMENTPRESELECTIONCAMPAGNEBTA_Delete.Remove(ELEMENTPRESELECTION_);
       //        }
       //    }
       //}

       //public List<CsTournee> GetTournee()
       //{
       //    try
       //    {
       //        using (var context = new galadbEntities())
       //        {
       //            List<CsTournee> listeTournee = new List<CsTournee>();
       //            foreach (var item in context.TOURNEE.ToList())
       //            {
       //                listeTournee.Add(new CsTournee{
       //                    CENTRE = item.CENTRE,
       //                    DATECREATION = item.DATECREATION,
       //                    DATEMODIFICATION = item.DATEMODIFICATION,
       //                    FK_IDCENTRE = item.FK_IDCENTRE,
       //                    FK_IDRELEVEUR = item.FK_IDRELEVEUR,
       //                    CODE  = item.CODE,
       //                    LIBELLE = item.LIBELLE,
       //                    LOCALISATION = item.LOCALISATION,
       //                    MATRICULEPIA = item.MATRICULEPIA,
       //                    PK_ID = item.PK_ID,
       //                    PRIORITE = item.PRIORITE,
       //                    USERCREATION = item.USERCREATION,
       //                    USERMODIFICATION = item.USERMODIFICATION
       //                });
       //            }
       //            return listeTournee;
       //        }
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}

       //public List<CsREFFAMILLEANOMALIEBTA> GetFamilleAnomalie()
       //{
       //    try
       //    {
       //        using (var context = new galadbEntities())
       //        {
       //            List<CsREFFAMILLEANOMALIEBTA> listeFAMILLEANOMALIEBTA = new List<CsREFFAMILLEANOMALIEBTA>();
       //            foreach (var item in context.REFFAMILLEANOMALIEBTA.ToList())
       //            {
       //                CsREFFAMILLEANOMALIEBTA FAMILLEANOMALIEBTA = new CsREFFAMILLEANOMALIEBTA
       //                {
       //                    ESTFRAUDE=item.ESTFRAUDE,
       //                    FAMILLEANOMALIE_ID=item.FAMILLEANOMALIE_ID,
       //                    LIBELLE=item.LIBELLE                           
       //                };
       //                FAMILLEANOMALIEBTA.REFTYPEANOMALIEBTA = new List<CsREFTYPEANOMALIEBTA>();
       //                  foreach (var item_ in item.REFTYPEANOMALIEBTA)
       //                  {
       //                      CsREFTYPEANOMALIEBTA TYPEANOMALIEBTA=  new CsREFTYPEANOMALIEBTA
       //                      {
       //                       FAMILLEANOMALIE_ID=item_.FAMILLEANOMALIE_ID,
       //                       LIBELLE=item_.LIBELLE,
       //                       SIEGEANOMALIE_ID=item_.SIEGEANOMALIE_ID,
       //                       TYPEANOMALIE_ID=item_.TYPEANOMALIE_ID
       //                       //REFFAMILLEANOMALIEBTA=Entities.ConvertObject<CsREFFAMILLEANOMALIEBTA,REFFAMILLEANOMALIEBTA>( item_.REFFAMILLEANOMALIEBTA),
       //                       //TBRESULTATCONTROLEBTA=Entities.ConvertObject<CsTBRESULTATCONTROLEBTA,TBRESULTATCONTROLEBTA>(item_.TBRESULTATCONTROLEBTA.ToList())
       //                      };
       //                      FAMILLEANOMALIEBTA.REFTYPEANOMALIEBTA.Add(TYPEANOMALIEBTA);
       //                  }
       //                 listeFAMILLEANOMALIEBTA.Add(FAMILLEANOMALIEBTA);
       //            }
       //            return listeFAMILLEANOMALIEBTA;
       //        }
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}

       //public bool SaveAnomalies(CsTBELEMENTLOTDECONTROLEBTA ELEMENTLOTDECONTROLEBTA, List<CsREFFAMILLEANOMALIEBTA> LISTEREFFAMILLEANOMALIEBTA,bool ESTFRAUDE=false)
       //{
       //    try
       //    {
       //        bool ContientAnomalies = false;
       //        List<REFTYPEANOMALIEBTA> LISTETYPEANOMALIEBTA = new List<REFTYPEANOMALIEBTA>();
       //        TBLOTDECONTROLEBTA LOT = new TBLOTDECONTROLEBTA();
       //        using (var context = new galadbEntities())
       //        {
       //            LOT=context.TBLOTDECONTROLEBTA.FirstOrDefault(l => l.LOT_ID == ELEMENTLOTDECONTROLEBTA.LOT_ID);
       //            LISTETYPEANOMALIEBTA = context.REFTYPEANOMALIEBTA.ToList();
       //        }
       //            List<TBRESULTATCONTROLEBTA> LISTE_RESULTATCONTROLEBTAToINSERT = new List<TBRESULTATCONTROLEBTA>();
       //            TBCONSTATFRAUDEBTA CONSTATFRAUDEBTA = new TBCONSTATFRAUDEBTA();
       //            CONSTATFRAUDEBTA.MATRICULEAGENTCONTROLEUR = LOT != null ? LOT.MATRICULEAGENTCONTROLEUR : "";
       //            CONSTATFRAUDEBTA.MATRICULEAGENTSAISIE = LOT != null ? LOT.MATRICULEAGENTCONTROLEUR : "";
       //            CONSTATFRAUDEBTA.CONSTATFRAUDE_ID = Guid.NewGuid();
       //            CONSTATFRAUDEBTA.DATECONTROLE = DateTime.Now;
       //            CONSTATFRAUDEBTA.DATESAISIE = DateTime.Now;
       //            CONSTATFRAUDEBTA.BRT_ID = ELEMENTLOTDECONTROLEBTA.BRT_ID;
       //            CONSTATFRAUDEBTA.CAMPAGNE_ID =LOT != null ?  LOT.CAMPAGNE_ID:Guid.NewGuid();
       //            CONSTATFRAUDEBTA.ESTFRAUDE = LISTEREFFAMILLEANOMALIEBTA.Select(f => f.ESTFRAUDE).Contains(true) ? true : false;
       //            CONSTATFRAUDEBTA.ESTISOLE = ESTFRAUDE;
       //            CONSTATFRAUDEBTA.ACTIONSDURANTLECONSTAT = "";

       //            CONSTATFRAUDEBTA.TBANOMALIESDETECTEESBTA = new List<TBANOMALIESDETECTEESBTA>();
       //            foreach (var REFFAMILLEANOMALIEBTA in LISTEREFFAMILLEANOMALIEBTA)
       //            {
                      
       //                foreach (var item in REFFAMILLEANOMALIEBTA.REFTYPEANOMALIEBTA)
       //                {
       //                    TBANOMALIESDETECTEESBTA ANOMALIESDETECTEESBTA = new TBANOMALIESDETECTEESBTA();
       //                    ANOMALIESDETECTEESBTA.BRT_ID = ELEMENTLOTDECONTROLEBTA.BRT_ID;
       //                    ANOMALIESDETECTEESBTA.CONSTATFRAUDE_ID = CONSTATFRAUDEBTA.CONSTATFRAUDE_ID;
       //                    ANOMALIESDETECTEESBTA.LIBELLEANOMALIE = item.LIBELLE;
       //                    ANOMALIESDETECTEESBTA.MATRICULEAGENTCONTROLE = CONSTATFRAUDEBTA.MATRICULEAGENTCONTROLEUR;
       //                    ANOMALIESDETECTEESBTA.DATECONTROLE =CONSTATFRAUDEBTA.DATECONTROLE.Value;
       //                    ANOMALIESDETECTEESBTA.DATESAISIE = CONSTATFRAUDEBTA.DATESAISIE.Value;
       //                    ANOMALIESDETECTEESBTA.MATRICULEAGENTSAISIE = CONSTATFRAUDEBTA.MATRICULEAGENTSAISIE;
       //                    ANOMALIESDETECTEESBTA.TYPEANOMALIE_ID = item.TYPEANOMALIE_ID;
       //                    ANOMALIESDETECTEESBTA.ANOMALIESDETECTEES_ID = Guid.NewGuid();
       //                    ANOMALIESDETECTEESBTA.LOT_ID = LOT != null ? LOT.LOT_ID : Guid.NewGuid();
       //                    CONSTATFRAUDEBTA.TBANOMALIESDETECTEESBTA.Add(ANOMALIESDETECTEESBTA);
       //                }
       //            }
                  
       //            using (galadbEntities context = new galadbEntities())
       //            {
       //                Entities.InsertEntity<TBCONSTATFRAUDEBTA>(CONSTATFRAUDEBTA, context);
       //                context.SaveChanges();
       //                ContientAnomalies = true;
       //            }
       //            return ContientAnomalies;
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}

       //public List<CsREFFAMILLEANOMALIEBTA> GetAnomalies(int BRT_ID, Guid? LOT_ID)
       //{
       //    List<REFFAMILLEANOMALIEBTA> LISTEFAMILLEANOMALIESBTA = new List<REFFAMILLEANOMALIEBTA>();
       //    List<REFTYPEANOMALIEBTA> LISTETYPEANOMALIEBTA = new List<REFTYPEANOMALIEBTA>();
       //    List<TBANOMALIESDETECTEESBTA> LISTEANOMALIESDETECTEESBTA = new List<TBANOMALIESDETECTEESBTA>();
       //    List<CsREFFAMILLEANOMALIEBTA> listeFAMILLEANOMALIEBTA = new List<CsREFFAMILLEANOMALIEBTA>();
       //    using (var context = new galadbEntities())
       //    {
       //        LISTEFAMILLEANOMALIESBTA = context.REFFAMILLEANOMALIEBTA.ToList();
       //        LISTETYPEANOMALIEBTA = context.REFTYPEANOMALIEBTA.ToList();
       //        if (LOT_ID!=null)
       //        {
       //            LISTEANOMALIESDETECTEESBTA = context.TBANOMALIESDETECTEESBTA.Where(a => a.LOT_ID == LOT_ID && a.BRT_ID == BRT_ID).ToList();
       //        }
       //        else
       //        {
       //            LISTEANOMALIESDETECTEESBTA = context.TBANOMALIESDETECTEESBTA.Where(a => a.BRT_ID == BRT_ID).ToList();
       //        }
       //    }

       //    foreach (var item in LISTEANOMALIESDETECTEESBTA)
       //    {
       //        RegrouperAnomalieParFamille(LISTEFAMILLEANOMALIESBTA, LISTETYPEANOMALIEBTA, listeFAMILLEANOMALIEBTA, item);

       //    }
       //    return listeFAMILLEANOMALIEBTA;
       //}

       //private static void RegrouperAnomalieParFamille(List<REFFAMILLEANOMALIEBTA> LISTEFAMILLEANOMALIESBTA, List<REFTYPEANOMALIEBTA> LISTETYPEANOMALIEBTA, List<CsREFFAMILLEANOMALIEBTA> listeFAMILLEANOMALIEBTA, TBANOMALIESDETECTEESBTA item)
       //{
       //    if (listeFAMILLEANOMALIEBTA.FirstOrDefault(f => f.FAMILLEANOMALIE_ID == item.REFTYPEANOMALIEBTA.REFFAMILLEANOMALIEBTA.FAMILLEANOMALIE_ID) != null)
       //    {
       //        CsREFTYPEANOMALIEBTA TYPEANOMALIEBTA = new CsREFTYPEANOMALIEBTA
       //        {
       //            FAMILLEANOMALIE_ID = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).FAMILLEANOMALIE_ID,
       //            LIBELLE = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).LIBELLE,
       //            SIEGEANOMALIE_ID = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).SIEGEANOMALIE_ID,
       //            TYPEANOMALIE_ID = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).TYPEANOMALIE_ID
       //        };
       //        listeFAMILLEANOMALIEBTA.FirstOrDefault(f => f.FAMILLEANOMALIE_ID == item.REFTYPEANOMALIEBTA.REFFAMILLEANOMALIEBTA.FAMILLEANOMALIE_ID).REFTYPEANOMALIEBTA.Add(TYPEANOMALIEBTA);
       //    }
       //    else
       //    {
       //        CsREFFAMILLEANOMALIEBTA FAMILLEANOMALIEBTA = new CsREFFAMILLEANOMALIEBTA
       //        {
       //            ESTFRAUDE = LISTEFAMILLEANOMALIESBTA.FirstOrDefault(f => f.FAMILLEANOMALIE_ID == LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).FAMILLEANOMALIE_ID).ESTFRAUDE,
       //            FAMILLEANOMALIE_ID = LISTEFAMILLEANOMALIESBTA.FirstOrDefault(f => f.FAMILLEANOMALIE_ID == LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).FAMILLEANOMALIE_ID).FAMILLEANOMALIE_ID,
       //            LIBELLE = LISTEFAMILLEANOMALIESBTA.FirstOrDefault(f => f.FAMILLEANOMALIE_ID == LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).FAMILLEANOMALIE_ID).LIBELLE
       //        };
       //        FAMILLEANOMALIEBTA.REFTYPEANOMALIEBTA = new List<CsREFTYPEANOMALIEBTA>();
       //        CsREFTYPEANOMALIEBTA TYPEANOMALIEBTA = new CsREFTYPEANOMALIEBTA
       //        {
       //            FAMILLEANOMALIE_ID = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).FAMILLEANOMALIE_ID,
       //            LIBELLE = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).LIBELLE,
       //            SIEGEANOMALIE_ID = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).SIEGEANOMALIE_ID,
       //            TYPEANOMALIE_ID = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).TYPEANOMALIE_ID
       //        };
       //        FAMILLEANOMALIEBTA.REFTYPEANOMALIEBTA.Add(TYPEANOMALIEBTA);
       //        listeFAMILLEANOMALIEBTA.Add(FAMILLEANOMALIEBTA);
       //    }
       //}



       //public List<CsTBCONSTATFRAUDEBTA> GetConstat(int BRT_ID, Guid? LOT_ID, Guid? CAMPAGNE_ID)
       //{
       //    List<CsTBCONSTATFRAUDEBTA> TBCONSTATFRAUDEBTA = new List<CsTBCONSTATFRAUDEBTA>();

       //    List<REFFAMILLEANOMALIEBTA> LISTEFAMILLEANOMALIESBTA = new List<REFFAMILLEANOMALIEBTA>();
       //    List<REFTYPEANOMALIEBTA> LISTETYPEANOMALIEBTA = new List<REFTYPEANOMALIEBTA>();
       //    List<TBANOMALIESDETECTEESBTA> LISTEANOMALIESDETECTEESBTA = new List<TBANOMALIESDETECTEESBTA>();
       //    List<TBLOTDECONTROLEBTA> LISTELOTDECONTROLEBTA = new List<TBLOTDECONTROLEBTA>();
       //    using (var context = new galadbEntities())
       //    {

       //        LISTEFAMILLEANOMALIESBTA = context.REFFAMILLEANOMALIEBTA.ToList();
       //        LISTETYPEANOMALIEBTA = context.REFTYPEANOMALIEBTA.ToList();
       //        LISTELOTDECONTROLEBTA = context.TBLOTDECONTROLEBTA.ToList();

       //            var AnomalieParConstat1 = context.TBANOMALIESDETECTEESBTA.Where(a => a.BRT_ID == BRT_ID);
       //            if (LOT_ID!=null)
       //            {
       //                AnomalieParConstat1 = AnomalieParConstat1.Where(a => a.LOT_ID == LOT_ID);
       //            }
       //            if (CAMPAGNE_ID != null)
       //            {
       //                AnomalieParConstat1 = AnomalieParConstat1.Where(a => a.TBCONSTATFRAUDEBTA.CAMPAGNE_ID == CAMPAGNE_ID);
       //            }
       //            var AnomalieParConstat = AnomalieParConstat1.GroupBy(a =>new { a.TBCONSTATFRAUDEBTA, a.LOT_ID }, a => a,
       //                                                                        (key, an) => new { CONSTATFRAUDEBTA = key, ANOMALIESDETECTEES = an })
       //                                                                        .ToList();

       //            foreach (var item in AnomalieParConstat)
       //            {
       //                List<CsREFFAMILLEANOMALIEBTA> listeFAMILLEANOMALIEBTA = new List<CsREFFAMILLEANOMALIEBTA>();
       //                CsTBCONSTATFRAUDEBTA CONSTATFRAUDEBTA = new CsTBCONSTATFRAUDEBTA();
       //                CONSTATFRAUDEBTA.LISTFAMILLEANOMALIEBTA = new List<CsREFFAMILLEANOMALIEBTA>();
       //                CONSTATFRAUDEBTA.ACTIONSDURANTLECONSTAT = item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.ACTIONSDURANTLECONSTAT;
       //                CONSTATFRAUDEBTA.CAMPAGNE_ID = item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.CAMPAGNE_ID != null ? item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.CAMPAGNE_ID : new Guid();
       //                CONSTATFRAUDEBTA.CONSTATFRAUD_PREC_ID = item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.CONSTATFRAUD_PREC_ID != null ? item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.CONSTATFRAUD_PREC_ID.Value : new Guid();
       //                CONSTATFRAUDEBTA.CONSTATFRAUDE_ID = item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.CAMPAGNE_ID != null ? item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.CAMPAGNE_ID.Value : new Guid();
       //                CONSTATFRAUDEBTA.CONTRAT_ID = item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.CONTRAT_ID != null ? item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.CONTRAT_ID.Value : new Guid();
       //                CONSTATFRAUDEBTA.DATECONTROLE = item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.DATECONTROLE != null ? item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.DATECONTROLE.Value : DateTime.MinValue;
       //                CONSTATFRAUDEBTA.DATESAISIE = item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.DATESAISIE != null ? item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.DATESAISIE.Value : DateTime.MinValue;
       //                CONSTATFRAUDEBTA.ID_FICHESCANNEE = item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.ID_FICHESCANNEE != null ? item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.ID_FICHESCANNEE.Value : new Guid();
       //                CONSTATFRAUDEBTA.MATRICULEAGENTCONTROLEUR = !string.IsNullOrWhiteSpace(item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.MATRICULEAGENTCONTROLEUR) ? item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.MATRICULEAGENTCONTROLEUR : "";
       //                CONSTATFRAUDEBTA.MATRICULEAGENTSAISIE = !string.IsNullOrWhiteSpace(item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.MATRICULEAGENTSAISIE) ? item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.MATRICULEAGENTSAISIE : "";
       //                CONSTATFRAUDEBTA.NUMERO_AVISDEPASSAGE = !string.IsNullOrWhiteSpace(item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.NUMERO_AVISDEPASSAGE) ? item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.NUMERO_AVISDEPASSAGE : "";
       //                CONSTATFRAUDEBTA.PRESENCE_FORCEORDRE = item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.PRESENCE_FORCEORDRE != null ? item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.PRESENCE_FORCEORDRE : false;
       //                CONSTATFRAUDEBTA.RESULTATCONTROLEPOSTFRAUDE_ID = item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.RESULTATCONTROLEPOSTFRAUDE_ID != null ? item.CONSTATFRAUDEBTA.TBCONSTATFRAUDEBTA.RESULTATCONTROLEPOSTFRAUDE_ID : new Guid();
       //                var LOT=LISTELOTDECONTROLEBTA.FirstOrDefault(l => l.LOT_ID == item.CONSTATFRAUDEBTA.LOT_ID);
       //                if (LOT != null)
       //                {
       //                    CsTBLOTDECONTROLEBTA lelot = new CsTBLOTDECONTROLEBTA
       //                    {
       //                        CAMPAGNE_ID = LOT.CAMPAGNE_ID,
       //                        DATECREATION = LOT.DATECREATION,
       //                        CRITERE_GROUPEDEFACTURATION = LOT.CRITERE_GROUPEDEFACTURATION,
       //                        //CRITERE_IDTOURNEE = LOT.CRITERE_IURNEE,
       //                        CRITERE_TYPECLIENT = LOT.CRITERE_TYPECLIENT,
       //                        CRITERE_TYPECOMPTEUR = LOT.CRITERE_TYPECOMPTEUR,
       //                        CRITERE_TYPETARIF = LOT.CRITERE_TYPETARIF,
       //                        DATEFERMETURE = LOT.DATEFERMETURE,
       //                        LIBELLE_LOT = LOT.LIBELLE_LOT,
       //                        LOT_ID = LOT.LOT_ID,
       //                        MATRICULEAGENTCONTROLEUR = LOT.MATRICULEAGENTCONTROLEUR,
       //                        MATRICULEAGENTCREATION = LOT.MATRICULEAGENTCREATION,
       //                        NBREELEMENTSDULOT = LOT.NBREELEMENTSDULOT,
       //                        STATUTLOT_ID = LOT.STATUTLOT_ID
       //                    };
       //                    CONSTATFRAUDEBTA.LOTDECONTROLEBTA = lelot;
       //                }
       //                else
       //                {
       //                    CONSTATFRAUDEBTA.LOTDECONTROLEBTA = new CsTBLOTDECONTROLEBTA();
       //                }
                       
       //                foreach (var _item in item.ANOMALIESDETECTEES)
       //                {
       //                    RegrouperAnomalieParFamille(LISTEFAMILLEANOMALIESBTA, LISTETYPEANOMALIEBTA, listeFAMILLEANOMALIEBTA, _item);
       //                }
       //                CONSTATFRAUDEBTA.LISTFAMILLEANOMALIEBTA = listeFAMILLEANOMALIEBTA;
       //                TBCONSTATFRAUDEBTA.Add(CONSTATFRAUDEBTA);
       //            }
               
       //    }
       //    return TBCONSTATFRAUDEBTA;
       //}

       //private static List<CsREFFAMILLEANOMALIEBTA> GetFamilleAnomalie(int BRT_ID, Guid LOT_ID)
       //{
       //    List<REFFAMILLEANOMALIEBTA> LISTEFAMILLEANOMALIESBTA = new List<REFFAMILLEANOMALIEBTA>();
       //    List<REFTYPEANOMALIEBTA> LISTETYPEANOMALIEBTA = new List<REFTYPEANOMALIEBTA>();
       //    List<TBANOMALIESDETECTEESBTA> LISTEANOMALIESDETECTEESBTA = new List<TBANOMALIESDETECTEESBTA>();
       //    List<CsREFFAMILLEANOMALIEBTA> listeFAMILLEANOMALIEBTA = new List<CsREFFAMILLEANOMALIEBTA>();
       //    using (var context = new galadbEntities())
       //    {
       //        LISTEFAMILLEANOMALIESBTA = context.REFFAMILLEANOMALIEBTA.ToList();
       //        LISTETYPEANOMALIEBTA = context.REFTYPEANOMALIEBTA.ToList();
       //        LISTEANOMALIESDETECTEESBTA = context.TBANOMALIESDETECTEESBTA.Where(a => a.LOT_ID == LOT_ID && a.BRT_ID == BRT_ID).ToList();
       //    }

       //    foreach (var item in LISTEANOMALIESDETECTEESBTA)
       //    {
       //        if (listeFAMILLEANOMALIEBTA.FirstOrDefault(f => f.FAMILLEANOMALIE_ID == item.REFTYPEANOMALIEBTA.REFFAMILLEANOMALIEBTA.FAMILLEANOMALIE_ID) != null)
       //        {
       //            CsREFTYPEANOMALIEBTA TYPEANOMALIEBTA = new CsREFTYPEANOMALIEBTA
       //            {
       //                FAMILLEANOMALIE_ID = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).FAMILLEANOMALIE_ID,
       //                LIBELLE = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).LIBELLE,
       //                SIEGEANOMALIE_ID = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).SIEGEANOMALIE_ID,
       //                TYPEANOMALIE_ID = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).TYPEANOMALIE_ID
       //            };
       //            listeFAMILLEANOMALIEBTA.FirstOrDefault(f => f.FAMILLEANOMALIE_ID == item.REFTYPEANOMALIEBTA.REFFAMILLEANOMALIEBTA.FAMILLEANOMALIE_ID).REFTYPEANOMALIEBTA.Add(TYPEANOMALIEBTA);
       //        }
       //        else
       //        {
       //            CsREFFAMILLEANOMALIEBTA FAMILLEANOMALIEBTA = new CsREFFAMILLEANOMALIEBTA
       //            {
       //                ESTFRAUDE = LISTEFAMILLEANOMALIESBTA.FirstOrDefault(f => f.FAMILLEANOMALIE_ID == LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).FAMILLEANOMALIE_ID).ESTFRAUDE,
       //                FAMILLEANOMALIE_ID = LISTEFAMILLEANOMALIESBTA.FirstOrDefault(f => f.FAMILLEANOMALIE_ID == LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).FAMILLEANOMALIE_ID).FAMILLEANOMALIE_ID,
       //                LIBELLE = LISTEFAMILLEANOMALIESBTA.FirstOrDefault(f => f.FAMILLEANOMALIE_ID == LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).FAMILLEANOMALIE_ID).LIBELLE
       //            };
       //            FAMILLEANOMALIEBTA.REFTYPEANOMALIEBTA = new List<CsREFTYPEANOMALIEBTA>();
       //            CsREFTYPEANOMALIEBTA TYPEANOMALIEBTA = new CsREFTYPEANOMALIEBTA
       //            {
       //                FAMILLEANOMALIE_ID = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).FAMILLEANOMALIE_ID,
       //                LIBELLE = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).LIBELLE,
       //                SIEGEANOMALIE_ID = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).SIEGEANOMALIE_ID,
       //                TYPEANOMALIE_ID = LISTETYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).TYPEANOMALIE_ID
       //            };
       //            FAMILLEANOMALIEBTA.REFTYPEANOMALIEBTA.Add(TYPEANOMALIEBTA);
       //            listeFAMILLEANOMALIEBTA.Add(FAMILLEANOMALIEBTA);
       //        }

       //    }
       //    return listeFAMILLEANOMALIEBTA;
       //}





       //public List<CsBrt> GetBranchement()
       //{
       //    try
       //    { 
       //        List<BRT> LISTEBRT =new List<BRT>();
       //        List<CsBrt> LISTEBRT_ToReturn = new List<CsBrt>();
       //        List<CLIENT> client=new List<CLIENT>();
       //        using (var context=new galadbEntities())
       //        {
       //            LISTEBRT = context.BRT.Take(100).ToList();
       //            client=context.CLIENT.ToList();
       //        }

       //        foreach (var item in LISTEBRT)
       //        {
       //            CsBrt lebrt=new CsBrt();
       //            lebrt.ADRESSERESEAU=item.ADRESSERESEAU;
       //            lebrt.ANFAB=item.ANFAB;
       //            lebrt.APPTRANSFO=item.APPTRANSFO;
       //            lebrt.CATBRT=item.CATBRT;
       //            //lebrt.CENTRE=item.CENTRE;
       //            //lebrt.CLIENT=item.CLIENT;
       //            lebrt.CODEDBRT=item.CODEBRT;
       //            lebrt.CODEPOSTE=item.CODEPOSTE;
       //            lebrt.COEFPERTES=item.COEFPERTES;
       //            lebrt.DATE_EDITION=item.DATECREATION;
       //            lebrt.DATECREATION=item.DATECREATION;
       //            lebrt.DATEMODIFICATION=item.DATEMODIFICATION;
       //            //lebrt.DIAMBRT=item.DIAMBRT;
       //            lebrt.DMAJ=item.DMAJ;
       //            lebrt.DRAC=item.DRAC;
       //            lebrt.DRES=item.DRES;
       //            lebrt.FK_IDCENTRE=item.FK_IDCENTRE;
       //            lebrt.FK_IDDIAMETRE=item.FK_IDDIAMETRE;
       //            lebrt.FK_IDPRODUIT=item.FK_IDPRODUIT;
       //            lebrt.FK_IDTOURNEE=item.FK_IDTOURNEE;
       //            lebrt.LATITUDE=item.LATITUDE;
       //            lebrt.LONGBRT=item.LONGBRT;
       //            lebrt.LONGITUDE=item.LONGITUDE;
       //            lebrt.MARQUETRANSFO=item.MARQUETRANSFO;
       //            lebrt.NATBRT=item.NATBRT;
       //            lebrt.NBPOINT=item.NBPOINT;
       //            lebrt.ORDTOUR=item.ORDTOUR;
       //            lebrt.PERTES=item.PERTES;
       //            lebrt.PK_ID=item.PK_ID;
       //            //lebrt.PRODUIT=item.PRODUIT;
       //            lebrt.PUISSANCEINSTALLEE=item.PUISSANCEINSTALLEE;
       //            lebrt.RESEAU=item.RESEAU;
       //            lebrt.SERVICE=item.SERVICE;
       //            //lebrt.TOURNEE=item.TOURNEE;
       //            lebrt.TRANSFORMATEUR=item.TRANSFORMATEUR;
       //            lebrt.TRONCON=item.TRONCON;
       //            //lebrt.TYPECOMPTAGE=item.TYPECOMPTAGE;
       //            lebrt.USERCREATION=item.USERCREATION;
       //            lebrt.USERMODIFICATION=item.USERMODIFICATION;
       //            lebrt.NOMABON = client.FirstOrDefault(c => c.PK_ID == item.FK_IDCLIENT).NOMABON;
       //            LISTEBRT_ToReturn.Add(lebrt);
                   
       //        }

       //        return LISTEBRT_ToReturn;
       //    }
       //    catch (Exception ex)
       //    {
               
       //        throw ex;
       //    }
       //}

       //public List<CsTBCONSTATFRAUDEBTA> GetconstatFraud(string code_centre, string txt_Branchement)
       //{
       //    try
       //    {
       //        List<CsTBCONSTATFRAUDEBTA> listeTBCONSTATFRAUDEBTA = new List<CsTBCONSTATFRAUDEBTA>();
       //        CsTBCONSTATFRAUDEBTA TBCONSTATFRAUDEBTA_;

       //        using (var context = new galadbEntities())
       //        {
       //             var VueTBCONSTATFRAUDEBTA_ =  from constat in context.TBCONSTATFRAUDEBTA
       //                                           join camp in context.TBCAMPAGNECONTROLEBTA
       //                                           on constat.CAMPAGNE_ID equals camp.CAMPAGNE_ID
       //                                           join brt in context.BRT
       //                                           on constat.BRT_ID equals brt.PK_ID
       //                                           select new { constat,camp.CODECENTRE,brt.CODEBRT };

       //             if (!string.IsNullOrWhiteSpace(code_centre))
       //             {
       //                 int centre=int.Parse(code_centre);
       //                 VueTBCONSTATFRAUDEBTA_ = VueTBCONSTATFRAUDEBTA_.Where(constat => constat.CODECENTRE == centre);
       //             } 
       //            if (!string.IsNullOrWhiteSpace(txt_Branchement))
       //             {
       //                 VueTBCONSTATFRAUDEBTA_=VueTBCONSTATFRAUDEBTA_.Where(constat=>constat.CODEBRT==txt_Branchement);
       //             }

       //            foreach (var item in VueTBCONSTATFRAUDEBTA_.Select(constat=>constat.constat).ToList())
       //             {
       //                  TBCONSTATFRAUDEBTA_=new CsTBCONSTATFRAUDEBTA{
       //                      ACTIONSDURANTLECONSTAT=item.ACTIONSDURANTLECONSTAT,
       //                      CAMPAGNE_ID=item.CAMPAGNE_ID,
       //                      CONSTATFRAUDE_ID=item.CONSTATFRAUDE_ID,
       //                      CONTRAT_ID=item.CONTRAT_ID,
       //                      DATECONTROLE=item.DATECONTROLE,
       //                      DATESAISIE=item.DATESAISIE,
       //                      ID_FICHESCANNEE=item.ID_FICHESCANNEE,
       //                      MATRICULEAGENTCONTROLEUR=item.MATRICULEAGENTCONTROLEUR,
       //                      MATRICULEAGENTSAISIE=item.MATRICULEAGENTSAISIE,
       //                      NUMERO_AVISDEPASSAGE=item.NUMERO_AVISDEPASSAGE,
       //                      PRESENCE_FORCEORDRE=item.PRESENCE_FORCEORDRE,
       //                      RESULTATCONTROLEPOSTFRAUDE_ID=item.RESULTATCONTROLEPOSTFRAUDE_ID};
                       
       //                listeTBCONSTATFRAUDEBTA.Add(TBCONSTATFRAUDEBTA_);
       //             }
       //        }

       //        return listeTBCONSTATFRAUDEBTA;
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}

       //public List<CsBrt> GetBranchementByCritere(string code_centre, string txt_ref_Cli, string txt_Nom_Cli)
       //{
       //    try
       //    {
       //            List<BRT> LISTEBRT = new List<BRT>();
       //            List<CsBrt> LISTEBRT_ToReturn = new List<CsBrt>();
       //            List<CLIENT> client = new List<CLIENT>();
       //            //using (var context = new galadbEntities())
       //            //{
       //            //    LISTEBRT = context.BRT.Where(b=>b.CENTRE=="110").Take(100).ToList();
       //            //    LISTEBRT.Add(context.BRT.FirstOrDefault(b => b.CLIENT == "00000075552"));
       //            //    client = context.CLIENT.ToList();
       //            //}
       //            //if (!string.IsNullOrWhiteSpace(code_centre))
       //            //{
       //            //    LISTEBRT = LISTEBRT.Where(b => b.CENTRE == code_centre).ToList();
       //            //}
       //            //if (!string.IsNullOrWhiteSpace(txt_ref_Cli))
       //            //{
       //            //    LISTEBRT = LISTEBRT.Where(b => b.CLIENT == txt_ref_Cli).ToList();
       //            //}

       //            foreach (var item in LISTEBRT)
       //            {
       //                CsBrt lebrt = new CsBrt();
       //                lebrt.ADRESSERESEAU = item.ADRESSERESEAU;
       //                lebrt.ANFAB = item.ANFAB;
       //                lebrt.APPTRANSFO = item.APPTRANSFO;
       //                lebrt.CATBRT = item.CATBRT;
       //                //lebrt.CENTRE = item.CENTRE;
       //                //lebrt.CLIENT = item.CLIENT;
       //                lebrt.CODEDBRT = item.CODEBRT;
       //                lebrt.CODEPOSTE = item.CODEPOSTE;
       //                lebrt.COEFPERTES = item.COEFPERTES;
       //                lebrt.DATE_EDITION = item.DATECREATION;
       //                lebrt.DATECREATION = item.DATECREATION;
       //                lebrt.DATEMODIFICATION = item.DATEMODIFICATION;
       //                //lebrt.DIAMBRT = item.DIAMBRT;
       //                lebrt.DMAJ = item.DMAJ;
       //                lebrt.DRAC = item.DRAC;
       //                lebrt.DRES = item.DRES;
       //                lebrt.FK_IDCENTRE = item.FK_IDCENTRE;
       //                lebrt.FK_IDDIAMETRE = item.FK_IDDIAMETRE;
       //                lebrt.FK_IDPRODUIT = item.FK_IDPRODUIT;
       //                lebrt.FK_IDTOURNEE = item.FK_IDTOURNEE;
       //                lebrt.LATITUDE = item.LATITUDE;
       //                lebrt.LONGBRT = item.LONGBRT;
       //                lebrt.LONGITUDE = item.LONGITUDE;
       //                lebrt.MARQUETRANSFO = item.MARQUETRANSFO;
       //                lebrt.NATBRT = item.NATBRT;
       //                lebrt.NBPOINT = item.NBPOINT;
       //                lebrt.ORDTOUR = item.ORDTOUR;
       //                lebrt.PERTES = item.PERTES;
       //                lebrt.PK_ID = item.PK_ID;
       //                //lebrt.PRODUIT = item.PRODUIT;
       //                lebrt.PUISSANCEINSTALLEE = item.PUISSANCEINSTALLEE;
       //                lebrt.RESEAU = item.RESEAU;
       //                lebrt.SERVICE = item.SERVICE;
       //                //lebrt.TOURNEE = item.TOURNEE;
       //                lebrt.TRANSFORMATEUR = item.TRANSFORMATEUR;
       //                lebrt.TRONCON = item.TRONCON;
       //                //lebrt.TYPECOMPTAGE = item.TYPECOMPTAGE;
       //                lebrt.USERCREATION = item.USERCREATION;
       //                lebrt.USERMODIFICATION = item.USERMODIFICATION;
       //                lebrt.NOMABON = client.FirstOrDefault(c => c.PK_ID == item.FK_IDCLIENT).NOMABON;
       //                LISTEBRT_ToReturn.Add(lebrt);

       //            }
       //            if (!string.IsNullOrWhiteSpace(txt_Nom_Cli))
       //            {
       //                LISTEBRT_ToReturn = LISTEBRT_ToReturn.Where(b => b.NOMABON.Contains(txt_Nom_Cli)).ToList();
       //            }

       //            return LISTEBRT_ToReturn;
               
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}


       //public List<CsTBCONSTATFRAUDEBTA> GetAllConstat(int BRT_ID, Guid? LOT_ID)
       //{
       //    throw new NotImplementedException();
       //}
        
       //public List<CsREFABAQUE_CHOIXTC> LoadAllAbaques()
       //{
       //    try 
       //     {	        
       //         List<CsREFABAQUE_CHOIXTC> ABAQUE_ToReturn = new List<CsREFABAQUE_CHOIXTC>();
       //         List<REFABAQUE_CHOIXTC> ABAQUE = new List<REFABAQUE_CHOIXTC>();
       //         using (var context = new galadbEntities())
       //         {
       //             ABAQUE = context.REFABAQUE_CHOIXTC.ToList();
               
       //             foreach (var item in ABAQUE)
       //             {
       //                 CsREFABAQUE_CHOIXTC leABAQUE = new CsREFABAQUE_CHOIXTC();
       //                 leABAQUE.ABAQUE_ID = item.ABAQUE_ID.ToString();
       //                 leABAQUE.ABAQUE_LIBELLE = item.ABAQUE_LIBELLE;
       //                 leABAQUE.NIVEAUTENSION_ID = item.NIVEAUTENSION_ID.Value.ToString();
       //                 leABAQUE.NIVEAUTENSION_LIBELLE = context.REFNIVEAUDETENSIONHTA.FirstOrDefault(n=>n.NIVEAUTENSION_ID==item.NIVEAUTENSION_ID).NIVEAUTENSION_LIBELLE;
       //                 leABAQUE.TYPECOMPTAGEHTA_LIBELLE = String.Join(",", context.REFTYPECOMPTAGEHTA.Where(type => type.REFABAQUE_CHOIXTC.Select(a => a.ABAQUE_ID).Contains(item.ABAQUE_ID)).Select(t => t.TYPECOMPTAGEHTA_LIBELLE).ToArray());
       //                 ABAQUE_ToReturn.Add(leABAQUE);
       //             }
       //         }
       //         return ABAQUE_ToReturn;
       //     }
       //     catch (Exception ex)
       //     {
		
       //         throw ex;
       //     }
           

       //}
        
       //public List<CsREFNIVEAUDETENSIONHTA> LoadAllNiveauTension()
       //{
       //    try
       //    {
       //        List<CsREFNIVEAUDETENSIONHTA> NIVEAUDETENSIONHTA_ToReturn = new List<CsREFNIVEAUDETENSIONHTA>();
       //        List<REFNIVEAUDETENSIONHTA> NIVEAUDETENSIONHTA = new List<REFNIVEAUDETENSIONHTA>();
       //        using (var context = new galadbEntities())
       //        {
       //            NIVEAUDETENSIONHTA = context.REFNIVEAUDETENSIONHTA.ToList();

       //            foreach (var item in NIVEAUDETENSIONHTA)
       //            {
       //                CsREFNIVEAUDETENSIONHTA leNIVEAUDETENSIONHTA = new CsREFNIVEAUDETENSIONHTA();
       //                leNIVEAUDETENSIONHTA.NIVEAUTENSION_ID = item.NIVEAUTENSION_ID.ToString();
       //                leNIVEAUDETENSIONHTA.NIVEAUTENSION_LIBELLE = item.NIVEAUTENSION_LIBELLE;
       //                NIVEAUDETENSIONHTA_ToReturn.Add(leNIVEAUDETENSIONHTA);
       //            }
       //        }
       //        return NIVEAUDETENSIONHTA_ToReturn;
       //    }
       //    catch (Exception ex)
       //    {

       //        throw ex;
       //    }
       //}

       //public List<CsREFTYPECOMPTAGEHTA> loadAllTypComptage()
       //{
       //    try
       //    {
       //        List<CsREFTYPECOMPTAGEHTA> REFTYPECOMPTAGEHTA_ToReturn = new List<CsREFTYPECOMPTAGEHTA>();
       //        List<REFTYPECOMPTAGEHTA> REFTYPECOMPTAGEHTA = new List<REFTYPECOMPTAGEHTA>();
       //        using (var context = new galadbEntities())
       //        {
       //            REFTYPECOMPTAGEHTA = context.REFTYPECOMPTAGEHTA.ToList();

       //            foreach (var item in REFTYPECOMPTAGEHTA)
       //            {
       //                CsREFTYPECOMPTAGEHTA leREFTYPECOMPTAGEHTA = new CsREFTYPECOMPTAGEHTA();
       //                leREFTYPECOMPTAGEHTA.TYPECOMPTAGEHTA_ID = item.TYPECOMPTAGEHTA_ID.ToString();
       //                leREFTYPECOMPTAGEHTA.TYPECOMPTAGEHTA_LIBELLE = item.TYPECOMPTAGEHTA_LIBELLE;
       //                REFTYPECOMPTAGEHTA_ToReturn.Add(leREFTYPECOMPTAGEHTA);
       //            }
       //        }
       //        return REFTYPECOMPTAGEHTA_ToReturn;
       //    }
       //    catch (Exception ex)
       //    {

       //        throw ex;
       //    }
       //}

       //public List<CsREFLIGNEABAQUE_CHOIXTC> LoadPlageDePuissance()
       //{
       //    try
       //    {
       //        List<CsREFLIGNEABAQUE_CHOIXTC> REFLIGNEABAQUE_CHOIXTC_ToReturn = new List<CsREFLIGNEABAQUE_CHOIXTC>();
       //        List<REFLIGNEABAQUE_CHOIXTC> REFLIGNEABAQUE_CHOIXTC = new List<REFLIGNEABAQUE_CHOIXTC>();
       //        using (var context = new galadbEntities())
       //        {
       //            REFLIGNEABAQUE_CHOIXTC = context.REFLIGNEABAQUE_CHOIXTC.ToList();

       //            foreach (var item in REFLIGNEABAQUE_CHOIXTC)
       //            {
       //                CsREFLIGNEABAQUE_CHOIXTC leREFLIGNEABAQUE_CHOIXTC = new CsREFLIGNEABAQUE_CHOIXTC();
       //                leREFLIGNEABAQUE_CHOIXTC.ABAQUE_ID = item.ABAQUE_ID.ToString();
       //                leREFLIGNEABAQUE_CHOIXTC.PS_BORNEINF = item.PS_BORNEINF;
       //                leREFLIGNEABAQUE_CHOIXTC.PS_BORNESUP = item.PS_BORNESUP;
       //                leREFLIGNEABAQUE_CHOIXTC.RAPPORTTC_INTENTREE = item.RAPPORTTC_INTENTREE;
       //                leREFLIGNEABAQUE_CHOIXTC.RAPPORTTC_INTSORTIE = item.RAPPORTTC_INTSORTIE;
       //                leREFLIGNEABAQUE_CHOIXTC.PLAGE_PS = item.PS_BORNEINF.ToString() + " à " + item.PS_BORNESUP;
       //                leREFLIGNEABAQUE_CHOIXTC.PLAGE_RAPPORTTC = item.RAPPORTTC_INTSORTIE + " / " + item.RAPPORTTC_INTENTREE;
       //                REFLIGNEABAQUE_CHOIXTC_ToReturn.Add(leREFLIGNEABAQUE_CHOIXTC);
       //            }
       //        }
       //        return REFLIGNEABAQUE_CHOIXTC_ToReturn;
       //    }
       //    catch (Exception ex)
       //    {

       //        throw ex;
       //    }
       //}

       //public bool SaveAllAbaques(List<CsREFABAQUE_CHOIXTC> ListeAbaques)
       //{
       //    try
       //    {
       //        List<REFABAQUE_CHOIXTC> ABAQUE_ToReturnToSave = new List<REFABAQUE_CHOIXTC>();
       //        List<REFABAQUE_CHOIXTC> ABAQUE_ToReturnToUpdate = new List<REFABAQUE_CHOIXTC>();
       //        List<REFABAQUE_CHOIXTC> ABAQUE_ToReturnToDelete = new List<REFABAQUE_CHOIXTC>();

       //        List<CsREFABAQUE_CHOIXTC> ABAQUE = new List<CsREFABAQUE_CHOIXTC>();
       //        using (var context = new galadbEntities())
       //        {
       //            List<REFTYPECOMPTAGEHTA> REFTYPECOMPTAGEHTA=context.REFTYPECOMPTAGEHTA.ToList();
       //            ABAQUE = ListeAbaques;

       //            foreach (var item in ABAQUE)
       //            {
       //                REFABAQUE_CHOIXTC leABAQUE = new REFABAQUE_CHOIXTC();

       //                if (item.ABAQUE_ID != null)
       //                { leABAQUE.ABAQUE_ID = int.Parse(item.ABAQUE_ID); }

       //                leABAQUE.ABAQUE_LIBELLE = item.ABAQUE_LIBELLE;
       //                leABAQUE.NIVEAUTENSION_ID = int.Parse(item.NIVEAUTENSION_ID);

       //                foreach (var item_ in item.LIST_REFLIGNEABAQUE_CHOIXTC)
       //                {
       //                    REFLIGNEABAQUE_CHOIXTC leREFLIGNEABAQUE_CHOIXTC = new REFLIGNEABAQUE_CHOIXTC();

       //                    leREFLIGNEABAQUE_CHOIXTC.PS_BORNEINF = item_.PS_BORNEINF;
       //                    leREFLIGNEABAQUE_CHOIXTC.PS_BORNESUP = item_.PS_BORNESUP;
       //                    leREFLIGNEABAQUE_CHOIXTC.RAPPORTTC_INTENTREE = item_.RAPPORTTC_INTENTREE;
       //                    leREFLIGNEABAQUE_CHOIXTC.RAPPORTTC_INTSORTIE = item_.RAPPORTTC_INTSORTIE;

       //                    leABAQUE.REFLIGNEABAQUE_CHOIXTC.Add(leREFLIGNEABAQUE_CHOIXTC);
       //                }

       //                foreach (var item__ in item.TYPECOMPTAGEHTA_LIBELLE.Split(','))
       //                {
       //                     REFTYPECOMPTAGEHTA leREFTYPECOMPTAGEHTA = new REFTYPECOMPTAGEHTA();

       //                    leREFTYPECOMPTAGEHTA = REFTYPECOMPTAGEHTA.FirstOrDefault(t=>t.TYPECOMPTAGEHTA_LIBELLE==item__);

       //                    leABAQUE.REFTYPECOMPTAGEHTA.Add(leREFTYPECOMPTAGEHTA);
       //                } 

       //                if (item.TRANSAC_STATE==1)
       //                {
       //                    ABAQUE_ToReturnToSave.Add(leABAQUE);
       //                }
       //                if (item.TRANSAC_STATE == 2)
       //                {
       //                    ABAQUE_ToReturnToUpdate.Add(leABAQUE);
       //                }
       //                if (item.TRANSAC_STATE == 3)
       //                {
       //                    ABAQUE_ToReturnToDelete.Add(leABAQUE);
       //                }
       //            }

       //            Entities.UpdateEntity<REFABAQUE_CHOIXTC>(ABAQUE_ToReturnToUpdate, context);
       //            Entities.InsertEntity<REFABAQUE_CHOIXTC>(ABAQUE_ToReturnToSave, context);
       //            Entities.DeleteEntity<REFABAQUE_CHOIXTC>(ABAQUE_ToReturnToDelete, context);


       //            context.SaveChanges();
       //        }
       //        return true;
       //    }
       //    catch (Exception ex)
       //    {

       //        throw ex;
       //    }
       //}

       //#region Statistique

       //public List<CsAnomaliesDetecteesBTA> Stat_LoadConstatparFamille(DateTime debut, DateTime fin, string codecentre, string codefamille, string codetype)
       //{
       //    try
       //    {
       //        List<CsAnomaliesDetecteesBTA> ListAnomaliesDetecteesBTA = new List<CsAnomaliesDetecteesBTA>();

       //        List<TBCAMPAGNECONTROLEBTA> CAMPAGNE_ID = new List<TBCAMPAGNECONTROLEBTA>();
       //        List<TBANOMALIESDETECTEESBTA> ANOMALIESDETECTEESBTA = new List<TBANOMALIESDETECTEESBTA>();
       //        List<TBCONSTATFRAUDEBTA> CONSTATFRAUDEBTA = new List<TBCONSTATFRAUDEBTA>();
       //        List<REFFAMILLEANOMALIEBTA> FAMILLEANOMALIEBTA = new List<REFFAMILLEANOMALIEBTA>();
       //        List<REFTYPEANOMALIEBTA> TYPEANOMALIEBTA = new List<REFTYPEANOMALIEBTA>();

       //        int? Codecentre = int.Parse(codecentre);

       //        using (var context = new galadbEntities())
       //        {
       //            TYPEANOMALIEBTA = context.REFTYPEANOMALIEBTA.ToList();
       //            FAMILLEANOMALIEBTA = context.REFFAMILLEANOMALIEBTA.ToList();
       //            CAMPAGNE_ID = context.TBCAMPAGNECONTROLEBTA.ToList();

       //            if (!string.IsNullOrWhiteSpace(codecentre))
       //            {
       //                CAMPAGNE_ID = CAMPAGNE_ID.Where(c => c.CODECENTRE == Codecentre) != null ? context.TBCAMPAGNECONTROLEBTA.Where(c => c.CODECENTRE == Codecentre).ToList() : new List<TBCAMPAGNECONTROLEBTA>();
       //            }

       //            var list_constat = context.TBCONSTATFRAUDEBTA.ToList();
       //            CONSTATFRAUDEBTA = list_constat != null ? list_constat.Where(cst => CAMPAGNE_ID.Select(c => c.CAMPAGNE_ID).Contains(cst.CAMPAGNE_ID.Value)).ToList() : new List<TBCONSTATFRAUDEBTA>();

       //            if (CONSTATFRAUDEBTA.Count() > 0)
       //            {
       //                var list_anomalie = context.TBANOMALIESDETECTEESBTA.ToList();
       //                ANOMALIESDETECTEESBTA = list_anomalie != null ? list_anomalie.Where(a => CONSTATFRAUDEBTA.Select(c => c.CONSTATFRAUDE_ID).Contains(a.CONSTATFRAUDE_ID.Value)).ToList() : new List<TBANOMALIESDETECTEESBTA>();
       //            }
       //            //ANOMALIESDETECTEESBTA=.Where(a=> CONSTATFRAUDEBTA.Select(c=>c.CONSTATFRAUDE_ID).Contains( a.CONSTATFRAUDE_ID.Value )).ToList();

       //            foreach (var item in ANOMALIESDETECTEESBTA)
       //            {
       //                CsAnomaliesDetecteesBTA AnomaliesDetecteesBTA = new CsAnomaliesDetecteesBTA();
       //                AnomaliesDetecteesBTA.FamilleAnomalie_ID = TYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).FAMILLEANOMALIE_ID;
       //                AnomaliesDetecteesBTA.FamilleAnomalie_Libelle = FAMILLEANOMALIEBTA.FirstOrDefault(f => f.FAMILLEANOMALIE_ID == AnomaliesDetecteesBTA.FamilleAnomalie_ID).LIBELLE;
       //                AnomaliesDetecteesBTA.TypeAnomalie_ID = item.TYPEANOMALIE_ID;
       //                AnomaliesDetecteesBTA.TypeAnomalie_Libelle = TYPEANOMALIEBTA.FirstOrDefault(t => t.TYPEANOMALIE_ID == item.TYPEANOMALIE_ID).LIBELLE;

       //                ListAnomaliesDetecteesBTA.Add(AnomaliesDetecteesBTA);
       //            }
       //        }

       //        if (!string.IsNullOrWhiteSpace(codefamille))
       //        {
       //            int Codefamille = int.Parse(codefamille);
       //            ListAnomaliesDetecteesBTA = ListAnomaliesDetecteesBTA.Where(f => f.FamilleAnomalie_ID == Codefamille).ToList();
       //        }
       //        if (!string.IsNullOrWhiteSpace(codetype))
       //        {
       //            int Codetype = int.Parse(codetype);
       //            ListAnomaliesDetecteesBTA = ListAnomaliesDetecteesBTA.Where(t => t.TypeAnomalie_ID == Codetype).ToList();
       //        }
       //        return ListAnomaliesDetecteesBTA;
       //    }
       //    catch (Exception ex)
       //    {

       //        throw ex;
       //    }
       //}
       //public List<CsCampagnesBTAAccessiblesParLUO> Stat_LoadCampgne(DateTime debut, DateTime fin, string codecentre)
       //{
       //    try
       //    {
       //        List<CsCampagnesBTAAccessiblesParLUO> ListCampagnesBTAAccessiblesParLUO = new List<CsCampagnesBTAAccessiblesParLUO>();
       //        List<CsTBCAMPAGNECONTROLEBTA> ListCampagne = new List<CsTBCAMPAGNECONTROLEBTA>();

       //        ListCampagne = GetCampagneBTAControle();
       //        using (var context = new galadbEntities())
       //        {
       //            var list_constat = context.TBCONSTATFRAUDEBTA.ToList();

       //            foreach (var item in ListCampagne)
       //            {
       //                CsCampagnesBTAAccessiblesParLUO CampagnesBTAAccessiblesParLUO = new CsCampagnesBTAAccessiblesParLUO();
       //                CampagnesBTAAccessiblesParLUO.Campagne_ID = item.CAMPAGNE_ID;
       //                CampagnesBTAAccessiblesParLUO.CodeCentre = item.CODECENTRE.Value.ToString();
       //                CampagnesBTAAccessiblesParLUO.CodeExploitation = item.CODEEXPLOITATION;
       //                CampagnesBTAAccessiblesParLUO.DateCreation = item.DATECREATION;
       //                CampagnesBTAAccessiblesParLUO.DateDebutControles = item.DATEDEBUTCONTROLES;
       //                CampagnesBTAAccessiblesParLUO.DateFinPrevue = item.DATEFINPREVUE;
       //                CampagnesBTAAccessiblesParLUO.DateModification = item.DATEMODIFICATION;
       //                CampagnesBTAAccessiblesParLUO.Libelle_Campagne = item.LIBELLE_CAMPAGNE;
       //                CampagnesBTAAccessiblesParLUO.MatriculeAgentCreation = item.MATRICULEAGENTCREATION;
       //                CampagnesBTAAccessiblesParLUO.MatriculeAgentDerniereModification = item.MATRICULEAGENTDERNIEREMODIFICATION;
       //                CampagnesBTAAccessiblesParLUO.NbreElements = item.NBREELEMENTS;
       //                CampagnesBTAAccessiblesParLUO.Statut_ID = item.STATUT_ID;
       //                //recupération de la population controlé
       //                var list_constat_camp = list_constat.Where(c => c.CAMPAGNE_ID == item.CAMPAGNE_ID).ToList();
       //                CampagnesBTAAccessiblesParLUO.NbreElementsControle = list_constat_camp.Count();
       //                CampagnesBTAAccessiblesParLUO.NbreElementsEnfraud = 0;
       //                foreach (var item_ in list_constat_camp)
       //                {
       //                    if (item_.ESTFRAUDE.Value)
       //                    {
       //                        CampagnesBTAAccessiblesParLUO.NbreElementsEnfraud++;
       //                    }

       //                }
       //                //recupération de la population controlé avec constat de fraude

       //                ListCampagnesBTAAccessiblesParLUO.Add(CampagnesBTAAccessiblesParLUO);
       //            }
       //        }
       //        return ListCampagnesBTAAccessiblesParLUO;
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}
       //public List<CsTBCONSTATFRAUDEBTA> LoadConstatCampagne(List<CsTBCAMPAGNECONTROLEBTA> TBCAMPAGNECONTROLEBTA)
       //{
       //    try
       //    {
       //        List<CsTBCONSTATFRAUDEBTA> list_TBCONSTATFRAUDEBTA = new List<CsTBCONSTATFRAUDEBTA>();
       //        var CAMPAGNE_ID = TBCAMPAGNECONTROLEBTA.Select(camp => camp.CAMPAGNE_ID).ToList();
       //        using (galadbEntities context = new galadbEntities())
       //        {
       //            var TBCONSTATFRAUDEBTA = context.TBCONSTATFRAUDEBTA.Where(c => CAMPAGNE_ID.Contains(c.CAMPAGNE_ID.Value)).ToList();
       //            foreach (var item in TBCONSTATFRAUDEBTA)
       //            {
       //                CsTBCONSTATFRAUDEBTA _item = new CsTBCONSTATFRAUDEBTA();
       //                _item.CAMPAGNE_ID = item.CAMPAGNE_ID;
       //                _item.BRT_ID = item.BRT_ID;
       //                _item.ACTIONSDURANTLECONSTAT = item.ACTIONSDURANTLECONSTAT;
       //                //_item.CONSTATFRAUD_PREC_ID = item.CONSTATFRAUD_PREC_ID.Value;
       //                _item.CONSTATFRAUDE_ID = item.CONSTATFRAUDE_ID;
       //                _item.CONTRAT_ID = item.CONTRAT_ID;
       //                _item.DATECONTROLE = item.DATECONTROLE;
       //                _item.DATESAISIE = item.DATESAISIE;
       //                _item.ESTFRAUDE = item.ESTFRAUDE;
       //                _item.ESTISOLE = item.ESTISOLE;
       //                _item.ID_FICHESCANNEE = item.ID_FICHESCANNEE;
       //                _item.MATRICULEAGENTCONTROLEUR = item.MATRICULEAGENTCONTROLEUR;
       //                _item.NUMERO_AVISDEPASSAGE = item.NUMERO_AVISDEPASSAGE;
       //                _item.PRESENCE_FORCEORDRE = item.PRESENCE_FORCEORDRE;
       //                _item.RESULTATCONTROLEPOSTFRAUDE_ID = item.RESULTATCONTROLEPOSTFRAUDE_ID;

       //                list_TBCONSTATFRAUDEBTA.Add(_item);
       //            }
       //        }
       //        return list_TBCONSTATFRAUDEBTA;
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}

       //#endregion

        public List<CsClient> GetClienteBTADuLotControle(CstbLotsDeControleBTA Lot)
        {
            List<CsClient> ListeClient=new List<CsClient>();

            using (galadbEntities context=new galadbEntities())
            {
                foreach (var item in Lot.ListElementLot)
                {
                    int Contrat_ID=int.Parse(item.Contrat_ID);
                    var client = (from cl in context.CLIENT
                                       join ag in context.AG on new { cl.FK_IDAG } equals new { FK_IDAG = ag.PK_ID }
                                       join brt in context.BRT on new { FK_IDAG = ag.PK_ID } equals new { brt.FK_IDAG }
                                       join c in context.CENTRE on new { CENTRE=cl.FK_IDCENTRE } equals new {CENTRE= c.PK_ID }
                                       where brt.PK_ID==Contrat_ID
                                       select cl).First();


                    CsClient obj=new CsClient();

                    obj.CENTRE = client.CENTRE != null ? client.CENTRE : "";
                    obj.DATECREATION = client.DATECREATION;
                    obj.DATEMODIFICATION = client.DATECREATION;
                    obj.FK_IDCENTRE = client.FK_IDCENTRE;
                    obj.PK_ID = client.PK_ID;
                    obj.USERCREATION = client.USERCREATION != null ? client.USERCREATION : "";
                    obj.USERMODIFICATION = client.USERMODIFICATION != null ? client.USERMODIFICATION : "";
                    obj.NOMABON = client.NOMABON != null ? client.NOMABON : "";
                    obj.REFCLIENT = client.REFCLIENT != null ? client.REFCLIENT : "";
                    obj.CODECONSO = client.CODECONSO != null ? client.CODECONSO : "";
                    obj.TELEPHONE = client.TELEPHONE != null ? client.TELEPHONE : "";
                    obj.CATEGORIE = !string.IsNullOrWhiteSpace(client.CATEGORIE) ? client.CATEGORIE : string.Empty;
                    obj.ADRMAND1 = !string.IsNullOrWhiteSpace(client.ADRMAND1) ? client.ADRMAND1 : string.Empty;
                    obj.ADRMAND2 = !string.IsNullOrWhiteSpace(client.ADRMAND2) ? client.ADRMAND2 : string.Empty;
                    obj.AGENTASSAINI = !string.IsNullOrWhiteSpace(client.AGENTASSAINI) ? client.AGENTASSAINI : string.Empty;
                    obj.AGENTFACTURE = !string.IsNullOrWhiteSpace(client.AGENTFACTURE) ? client.AGENTFACTURE : string.Empty;
                    obj.AGENTRECOUVR = !string.IsNullOrWhiteSpace(client.AGENTRECOUVR) ? client.AGENTRECOUVR : string.Empty;
                    obj.ANNAIS = !string.IsNullOrWhiteSpace(client.ANNAIS) ? client.ANNAIS : string.Empty;
                    //obj.AVANCE = !string.IsNullOrWhiteSpace(client.AVANCE) ? client.AVANCE : string.Empty;
                    //obj.BANQUE !string.IsNullOrWhiteSpace(client.BANQUE) ? client.BANQUE : string.Empty;
                    obj.BOITEPOSTAL = !string.IsNullOrWhiteSpace(client.BOITEPOSTAL) ? client.BOITEPOSTAL : string.Empty;
                    obj.BUREAU = !string.IsNullOrWhiteSpace(client.BUREAU) ? client.BUREAU : string.Empty;
                    obj.CATEGORIE = !string.IsNullOrWhiteSpace(client.CATEGORIE) ? client.CATEGORIE : string.Empty;
                    obj.CNI = !string.IsNullOrWhiteSpace(client.CNI) ? client.CNI : string.Empty;
                    obj.CODEIDENTIFICATIONNATIONALE = !string.IsNullOrWhiteSpace(client.CODEIDENTIFICATIONNATIONALE) ? client.CODEIDENTIFICATIONNATIONALE : string.Empty;
                    obj.CODERELANCE = !string.IsNullOrWhiteSpace(client.CODERELANCE) ? client.CODERELANCE : string.Empty;
                    //obj.CODESITE = !string.IsNullOrWhiteSpace(client.CODESITE) ? client.CODESITE : string.Empty;
                    obj.COMPTE = !string.IsNullOrWhiteSpace(client.COMPTE) ? client.COMPTE : string.Empty;
                    //obj.COMPTEUR =context.CANALISATION.FirstOrDefault(c => c.f== client.fk) client.COMPTEUR ;
                    obj.CONVENTION = !string.IsNullOrWhiteSpace(client.CONVENTION) ? client.CONVENTION : string.Empty;
                    obj.CPOS = !string.IsNullOrWhiteSpace(client.CPOS) ? client.CPOS : string.Empty;
                    //obj.CRET = !string.IsNullOrWhiteSpace(client.CRET) ? client.CRET : string.Empty;
                    //obj.DATECREATION = !string.IsNullOrWhiteSpace(client.CATEGORIE) ? client.CATEGORIE : string.Empty;
                    obj.DECRET = !string.IsNullOrWhiteSpace(client.DECRET) ? client.DECRET : string.Empty;
                    obj.DENABON = !string.IsNullOrWhiteSpace(client.DENABON) ? client.DENABON : string.Empty;
                    obj.DENMAND = !string.IsNullOrWhiteSpace(client.DENMAND) ? client.DENMAND : string.Empty;
                    obj.DINC = client.DINC;
                    obj.DMAJ =client.DMAJ;       
                    obj.DRES =context.ABON.FirstOrDefault(c => c.FK_IDCLIENT== client.PK_ID).DRES ;
                    obj.EMAIL = !string.IsNullOrWhiteSpace(client.EMAIL) ? client.EMAIL : string.Empty;
                    //obj.EXIGIBILITE =client.EXIGIBILITE ;
                    obj.LIBELLECATEGORIE = context.CATEGORIECLIENT.FirstOrDefault(c => c.PK_ID== client.FK_IDCATEGORIE).LIBELLE;
                    obj.LIBELLECENTRE = context.CENTRE.FirstOrDefault(c => c.PK_ID== client.FK_IDCENTRE).LIBELLE;
                    obj.LIBELLESITE = context.CENTRE.FirstOrDefault(c => c.PK_ID== client.FK_IDCENTRE).SITE.LIBELLE;
                    //obj.NOMABON = !string.IsNullOrWhiteSpace(client.NOMABON) ? client.NOMABON : string.Empty;
                    //obj.REFCLIENT = !string.IsNullOrWhiteSpace(client.CATEGORIE) ? client.CATEGORIE : string.Empty;
                    //obj.CATEGORIE = !string.IsNullOrWhiteSpace(client.CATEGORIE) ? client.CATEGORIE : string.Empty;
                    //obj.ADRMAND1


                    ListeClient.Add(obj);

                }
            }
            return ListeClient;
        }


        public bool InitialisationFraud(List<Galatee.Structure.Rpnt.CstbElementsLotDeControleBTA> ListElementLot)
        {
            try
            {
                int result = -1;
                 
                    using (galadbEntities contextiWeb=new galadbEntities() )
                    {
                      
                        foreach (var item in ListElementLot)
                        {
                            // Inertion client


                            #region récupération Info Utile

                            string CodeSite = string.Empty;
                            //Récupération info Client
                            int Contrat_ID=int.Parse(item.Contrat_ID);
                             var ClientiWeb = (from cl in contextiWeb.CLIENT
                                       join ag in contextiWeb.AG on new { cl.FK_IDAG } equals new { FK_IDAG = ag.PK_ID }
                                       join brt in contextiWeb.BRT on new { FK_IDAG = ag.PK_ID } equals new { brt.FK_IDAG }
                                       where brt.PK_ID==Contrat_ID
                                       select cl).First();

                            //Récupération info Ag
                            var AgiWeb = (from cl in contextiWeb.CLIENT
                                       join ag in contextiWeb.AG on new { cl.FK_IDAG } equals new { FK_IDAG = ag.PK_ID }
                                       join brt in contextiWeb.BRT on new { FK_IDAG = ag.PK_ID } equals new { brt.FK_IDAG }
                                       where brt.PK_ID==Contrat_ID
                                       select ag).First();

                            //Récupération info Abon
                             var AbonWeb = (from cl in contextiWeb.CLIENT
                                       join ab in contextiWeb.ABON on new { cl.PK_ID} equals new  {PK_ID=ab.FK_IDCLIENT  }
                                       join ag in contextiWeb.AG on new { cl.FK_IDAG } equals new { FK_IDAG = ag.PK_ID }
                                       join brt in contextiWeb.BRT on new { FK_IDAG = ag.PK_ID } equals new { brt.FK_IDAG }
                                       where brt.PK_ID==Contrat_ID
                                       select ab).First();

                             //Récupération info centre
                             var CentreiWeb = contextiWeb.CENTRE.FirstOrDefault(c => c.PK_ID == ClientiWeb.FK_IDCENTRE);
                             //var CentreFraude = contextfraude.CENTRE.FirstOrDefault(c => c.Code == CentreiWeb.CODE);

                             //Récupération info complementaire fraude(NB:Se rassurer que Rpnt existe dans la table SOURCECONTROLE
                             //var EtapeFraude = contextfraude.ETAPEFRAUDE.FirstOrDefault(c => c.NumEtape == 2);
                             var SourceControle = contextiWeb.SOURCECONTROLE.FirstOrDefault(c => c.Libelle.Trim() == "Campagne RPNT".Trim());
                             var typedemande = contextiWeb.TYPEDEMANDE .FirstOrDefault(c => c.CODE == Enumere.DemandeFraude );

                            #endregion

                        

                                #region Demande
                                DEMANDE Demande = new DEMANDE();

                                Demande.TYPEDEMANDE = typedemande.CODE ;
                                Demande.FK_IDTYPEDEMANDE = typedemande.PK_ID ;
                                Demande.FK_IDADMUTILISATEUR = 1;
                                Demande.CENTRE = ClientiWeb.CENTRE ;
                                Demande.PRODUIT = AbonWeb.PRODUIT ;
                                Demande.FK_IDPRODUIT  = AbonWeb.FK_IDPRODUIT ;
                                Demande.FK_IDCENTRE = ClientiWeb.FK_IDCENTRE ;
                                Demande.USERCREATION = "99999";
                                Demande.DATECREATION = DateTime.Now;
                                Demande.NUMDEM = AccueilProcedures.GetNumDevis(ClientiWeb.FK_IDCENTRE);
                                Entities.InsertEntity<DEMANDE>(Demande, contextiWeb);
                            
                                #endregion
                                #region Client

                                CLIENTFRAUDE ClientFraude = new CLIENTFRAUDE();

                                ClientFraude.Centre = ClientiWeb.CENTRE;
                                ClientFraude.Client = ClientiWeb.REFCLIENT;
                                ClientFraude.Commune = AgiWeb.COMMUNE;
                                ClientFraude.Email = ClientiWeb.EMAIL;
                                ClientFraude.Nomabon = ClientiWeb.NOMABON;
                                ClientFraude.Ordre = ClientiWeb.ORDRE;
                                ClientFraude.OrdreTournee = AgiWeb.ORDTOUR;
                                ClientFraude.Porte = AgiWeb.PORTE;
                                ClientFraude.PuissanceSouscrite = AbonWeb.PUISSANCE;
                                ClientFraude.Quartier = AgiWeb.QUARTIER;
                                ClientFraude.Rue = AgiWeb.RUE;
                                ClientFraude.Secteur = AgiWeb.SECTEUR;
                                ClientFraude.FK_IDSITE = ClientiWeb.CENTRE1.FK_IDCODESITE;
                                ClientFraude.Telephone = ClientiWeb.TELEPHONE;
                                ClientFraude.Tournee = AgiWeb.TOURNEE;
                                ClientFraude.FK_IDCENTRE = AgiWeb.FK_IDCENTRE;
                                ClientFraude.FK_IDCOMMUNE = AgiWeb.FK_IDCOMMUNE;
                                ClientFraude.FK_IDQUARTIER = AgiWeb.FK_IDQUARTIER;
                                ClientFraude.FK_SECTEUR = AgiWeb.FK_IDSECTEUR;
                                ClientFraude.FK_RUE = AgiWeb.FK_IDRUE;
                                ClientFraude.FK_IDCENTRE = AgiWeb.FK_IDCENTRE;
                                Entities.InsertEntity<CLIENTFRAUDE>(ClientFraude, contextiWeb);

                                #endregion
                                #region Dénonciateur
                            //Insertion Denonsiateur
                                //Aucun dénonciation identifiable dans ce cas ci.
                            #endregion

                                #region Fraude
                                FRAUDE Fraude = new FRAUDE();

                                Fraude.DateCreation = DateTime.Now;
                                Fraude.DateEtape = DateTime.Now;
                                Fraude.FK_IDCLIENTFRAUDE = ClientFraude.PK_ID;
                                Fraude.FK_IDSOURCECONTROLE = SourceControle.PK_ID;
                                Fraude.OrdreTraitement  = (byte)1;
                                Fraude.FicheTraitement = Demande.NUMDEM;
                                Fraude.FK_IDDEMANDE = Demande.PK_ID;
                                Entities.InsertEntity<FRAUDE>(Fraude, contextiWeb);
                                #endregion

                               result = contextiWeb.SaveChanges();
                               InsererMaDemandeBase(Demande.PK_ID.ToString(), typedemande.PK_ID, Demande.FK_IDCENTRE ,Demande.NUMDEM);
                            
                        }
                        return false ? result == -1 : true;

                    }
                 
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        public string InsererMaDemandeBase(string PKIDLaLigne, int CodeTDem, int FKIDCentreDemande, string codeDemande_TableTravail)
        {
            string _CodeDemande = string.Empty;
            bool _result = true;

            try
            {
                CsVwConfigurationWorkflowCentre wkfInfo = null;
                var LOperation = Entities.GetEntityListFromQuery<CsOperation>(ParamProcedure.LISTE_WKF_ALLOPERATIONS()).FirstOrDefault(f => f.CODE_TDEM == CodeTDem.ToString());
                if (null != LOperation)
                {
                    Guid WorkflowId = Guid.Empty;
                    List<CsVwConfigurationWorkflowCentre> lstConfig = Entities.GetEntityListFromQuery<CsVwConfigurationWorkflowCentre>(ParamProcedure.LISTE_WKF_CONFIGURATIONWORKFLOWCENTRE()).Where(t => t.CENTREID == FKIDCentreDemande && t.OPERATIONID == LOperation.PK_ID).ToList();
                    if (lstConfig != null && lstConfig.Count != 0)
                        wkfInfo = lstConfig.First();
                    WorkflowId = (null != wkfInfo) ? wkfInfo.PK_ID : Guid.Empty;

                    string pk_IDLine = PKIDLaLigne;
                    int cId = wkfInfo.CENTREID ;
                    Guid _wkfId = wkfInfo.PK_ID ;
                    Guid _opId = wkfInfo.OPERATIONID ;

                        CsRWorkflow rWKFCentre = null;
                        rWKFCentre = Entities.GetEntityListFromQuery<CsRWorkflow>(ParamProcedure.LISTE_WKF_RAFFECTATION_WKF_CENTRE(wkfInfo.PK_ID, wkfInfo.CENTREID, wkfInfo.OPERATIONID )).FirstOrDefault();
                        if (null != rWKFCentre)
                        {
                            CsCentre centre = Entities.GetEntityListFromQuery<CsCentre>(ParamProcedure.PARAM_CENTRE_RETOURNE()).Where(c => c.PK_ID == cId).FirstOrDefault();
                            CsWorkflow workflow =   Entities.GetEntityListFromQuery<CsWorkflow>(ParamProcedure.LISTE_WKF()).Where(w => w.PK_ID == _wkfId)
                                .FirstOrDefault();
                            CsOperation operation = Entities.GetEntityListFromQuery<CsOperation>(ParamProcedure.LISTE_WKF_ALLOPERATIONS()).Where(o => o.PK_ID == _opId)
                                .FirstOrDefault();

                            //Récupération du circuit
                            Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> lsRAffCircuit = new Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>();
                            lsRAffCircuit = SelectAllAffectationEtapeWorkflow(rWKFCentre.PK_ID);

                            List<CsRAffectationEtapeWorkflow> leCircuitNormal = lsRAffCircuit.Keys.Where(aff => !aff.FK_IDRETAPEWORKFLOWORIGINE.HasValue ||
                                aff.FK_IDRETAPEWORKFLOWORIGINE.Value == Guid.Empty)
                                .OrderBy(aff => aff.ORDRE)
                                .ToList();

                            CsRAffectationEtapeWorkflow _1sStep = leCircuitNormal.FirstOrDefault();
                            CsRAffectationEtapeWorkflow _2ndStep = leCircuitNormal.Where(step => step.ORDRE == _1sStep.ORDRE + 1)
                                .FirstOrDefault();
                            if (!string.IsNullOrEmpty(_1sStep.CONDITION))
                            {

                                string msgErr = string.Empty;
                                CsConditionBranchement laContion = new CsConditionBranchement();
                                foreach (var item in lsRAffCircuit)
                                {
                                    if (item.Key.PK_ID == _1sStep.PK_ID)
                                    {
                                        laContion = item.Value;
                                        break;
                                    }
                                }

                                //On utilise la condition pour transmettre
                                //bool onABienTeste = true;
                                //CsDemandeBase dmds = Entities.GetEntityFromQuery<CsDemandeBase>(AccueilProcedures.GetDemandeByNumIdDemande(int.Parse(pk_IDLine)));
                                //bool conditionRespecte = CheckIfConditionIsRespected<CsDemandeBase>(laContion.NOM, dmds,
                                //    ref msgErr, ref onABienTeste);
                                //if (onABienTeste)
                                //{
                                //    if (laContion.FK_IDETAPEVRAIE.HasValue && laContion.FK_IDETAPEFAUSE.HasValue)
                                //    {

                                //    }
                                //    else if (conditionRespecte && laContion.FK_IDETAPEVRAIE.HasValue && 0 != laContion.FK_IDETAPEVRAIE.Value)
                                //    {
                                //        CsRAffectationEtapeWorkflow leEtape = leCircuitNormal.FirstOrDefault(t => t.FK_IDETAPE == laContion.FK_IDETAPEVRAIE);
                                //        if (null == leEtape)
                                //            msgErr = "ERR : Aucune étape n'a été configurée avec cette ID";
                                //        else
                                //        {
                                //            _1sStep.FK_IDETAPE = leEtape.FK_IDETAPE;
                                //            CsRAffectationEtapeWorkflow EtapeSuiv = leCircuitNormal.FirstOrDefault(t => t.ORDRE == leEtape.ORDRE + 1);
                                //            _2ndStep.FK_IDETAPE = EtapeSuiv == null ? 0 : EtapeSuiv.FK_IDETAPE;
                                //        }
                                //    }
                                //}
                            }
                            //Création de la demande

                            CsDemandeWorkflow dmd = new CsDemandeWorkflow();

                            dmd.PK_ID = Guid.NewGuid();
                            dmd.DATECREATION = DateTime.Today.Date;
                            dmd.MATRICULEUSERCREATION = "9999";
                            dmd.ALLCENTRE = false;
                            dmd.FK_IDCENTRE = cId;
                            dmd.FK_IDOPERATION = _opId;
                            dmd.FK_IDRWORKLOW = rWKFCentre.PK_ID;
                            dmd.FK_IDSTATUS = (int)Enumere.STATUSDEMANDE.Initiee;
                            dmd.FK_IDWORKFLOW = _wkfId;
                            dmd.FK_IDLIGNETABLETRAVAIL = pk_IDLine;
                            dmd.FK_IDETAPEPRECEDENTE = 0;
                            dmd.FK_IDETAPEACTUELLE = _1sStep.FK_IDETAPE;
                            dmd.FK_IDETAPESUIVANTE = null != _2ndStep ? _2ndStep.FK_IDETAPE : 0;
                            dmd.CODE = centre.CODESITE + centre.CODE + DateTime.Today.Year + DateTime.Today.Month +
                                    DateTime.Now.Minute +
                                    DateTime.Now.Millisecond;
                            dmd.FK_IDTABLETRAVAIL = workflow.FK_IDTABLE_TRAVAIL.Value;
                            dmd.CODE_DEMANDE_TABLETRAVAIL = codeDemande_TableTravail;
                            dmd.DATEDERNIEREMODIFICATION = DateTime.Today.Date;


                            _result = Entities.InsertEntity<DEMANDE_WORFKLOW>(Entities.ConvertObject<DEMANDE_WORFKLOW, CsDemandeWorkflow>(dmd));
                            if (_result) /* tout es bon */
                            {
                                _CodeDemande = dmd.CODE;

                                //On récupère les emails pour notifier les utilisateurs de l'arrivée de la demande
                                KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> grpValidation = SelectAllGroupeValidation()
                                    .Where(g => g.Key.PK_ID == _1sStep.FK_IDGROUPEVALIDATIOIN)
                                    .FirstOrDefault();

                                #region Création de la copie

                                _result = CopieCicruitEtapeDemande(rWKFCentre.PK_ID, dmd.PK_ID, _CodeDemande);
                                if (!_result) _CodeDemande = "Une erreur s'est produite";

                                #endregion
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                _CodeDemande = ex.Message;
            }
            return "ERR : " + _CodeDemande;

        }
        public bool CopieCicruitEtapeDemande(Guid pRWKF, Guid pkDmd, string codeDemande)
        {
            try
            {
                //On récupère la demande
                CsDemandeWorkflow dmd = Entities.GetEntityListFromQuery<CsDemandeWorkflow>(ParamProcedure.LISTE_WKF_DEMANDE(codeDemande))
                    .FirstOrDefault();
                //On fait un select du circuit actuelle,
                //ensuite on le copie
                Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> leCircuit = SelectAllAffectationEtapeWorkflow(pRWKF);
                //La copie permet juste l'insertion avec le code de la demande et son id
                Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> copieCircuit = new Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement>();
                //Ce dictionaire va nous permettre de pouvoir garder en mémoire les anciens FK_IDAFFECTATIONETAPEORIGINE,
                //Pour les circuits de détournement, afin de pouvoir les remplacer par les nouveaux PK_ID
                Dictionary<Guid, Guid> OrigineGuidToReplace = new Dictionary<Guid, Guid>();

                foreach (KeyValuePair<CsRAffectationEtapeWorkflow, CsConditionBranchement> kVp in leCircuit)
                {
                    CsCopieDmdCircuit copieAff = new CsCopieDmdCircuit()
                    {
                        PK_ID = Guid.NewGuid(), //C'est une nouvelle insertion donc un nouveau GUID
                        CODE_DEMANDE = codeDemande,
                        FK_IDDEMANDE = pkDmd,
                        CODEETAPE = kVp.Key.CODEETAPE,
                        CONDITION = kVp.Key.CONDITION,
                        ETAPECONDITIONVRAIE = kVp.Key.ETAPECONDITIONVRAIE,
                        FK_IDETAPE = kVp.Key.FK_IDETAPE,
                        FK_IDGROUPEVALIDATIOIN = kVp.Key.FK_IDGROUPEVALIDATIOIN,
                        FK_IDRETAPEWORKFLOWORIGINE = kVp.Key.FK_IDRETAPEWORKFLOWORIGINE,
                        FK_RWORKFLOWCENTRE = kVp.Key.FK_RWORKFLOWCENTRE,
                        FROMCONDITION = kVp.Key.FROMCONDITION,
                        GROUPE_VALIDATION = kVp.Key.GROUPE_VALIDATION,
                        GROUPEVALIDATION = kVp.Key.GROUPEVALIDATION,
                        LIBELLEETAPE = kVp.Key.LIBELLEETAPE,
                        ORDRE = kVp.Key.ORDRE
                    };
                    //Ensuite on essaie de voir si on a un FK_IDRETAPEWORKFLOWORIGINE à remplacer
                    if (copieAff.FK_IDRETAPEWORKFLOWORIGINE.HasValue)
                    {
                        if (OrigineGuidToReplace.Keys.Contains(copieAff.FK_IDRETAPEWORKFLOWORIGINE.Value))
                            copieAff.FK_IDRETAPEWORKFLOWORIGINE = OrigineGuidToReplace[copieAff.FK_IDRETAPEWORKFLOWORIGINE.Value];
                    }
                    if (null != kVp.Value)
                    {
                        CsCopieDmdConditionBranchement copieCondition = new CsCopieDmdConditionBranchement()
                        {
                            PK_ID = Guid.NewGuid(),
                            FK_IDCOPIE_DMD_CIRCUIT = copieAff.PK_ID,
                            COLONNENAME = kVp.Value.COLONNENAME,
                            FK_IDETAPEFAUSE = kVp.Value.FK_IDETAPEFAUSE,
                            FK_IDETAPEVRAIE = kVp.Value.FK_IDETAPEVRAIE,
                            FK_IDTABLETRAVAIL = kVp.Value.FK_IDTABLETRAVAIL,
                            NOM = kVp.Value.NOM,
                            OPERATEUR = kVp.Value.OPERATEUR,
                            VALUE = kVp.Value.VALUE,
                            PEUT_TRANSMETTRE_SI_FAUX = kVp.Value.PEUT_TRANSMETTRE_SI_FAUX
                        };
                        copieCircuit.Add(copieAff, copieCondition);
                    }
                    else copieCircuit.Add(copieAff, null);
                }

                //On insère l'ID de la 1ere étape de son circuit à lui (la copie)
                if (null != dmd)
                {
                    var la1ereEtape = copieCircuit.Keys.Where(et => et.ORDRE == 1 && !et.FK_IDRETAPEWORKFLOWORIGINE.HasValue
                        && null == et.FK_IDRETAPEWORKFLOWORIGINE)
                        .FirstOrDefault();
                    if (null != la1ereEtape) dmd.FK_IDETAPECIRCUIT = la1ereEtape.PK_ID;
                }

                return InsertCopieCircuit(copieCircuit) && Entities.UpdateEntity<DEMANDE_WORFKLOW>(Entities.ConvertObject<DEMANDE_WORFKLOW, CsDemandeWorkflow>(dmd));  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool InsertCopieCircuit(Dictionary<CsCopieDmdCircuit, CsCopieDmdConditionBranchement> lsEtapes)
        {
            try
            {
                //On enregistre dabord les étapes
                bool insertEtape = Entities.InsertEntity<COPIE_DMD_CIRCUIT>(Entities.ConvertObject<COPIE_DMD_CIRCUIT,
                    CsCopieDmdCircuit>(lsEtapes.Keys.ToList()));
                bool insertCondition = true;
                try
                {
                    List<CsCopieDmdConditionBranchement> LesConditionsNonNulles = new List<CsCopieDmdConditionBranchement>();
                    foreach (var C in lsEtapes.Values)
                        if (null != C) LesConditionsNonNulles.Add(C);
                    if (insertEtape) insertCondition = Entities.InsertEntity<COPIE_DMD_CONDITIONBRANCHEMENT>(Entities.ConvertObject<COPIE_DMD_CONDITIONBRANCHEMENT,
                        CsCopieDmdConditionBranchement>(LesConditionsNonNulles));
                }
                catch (NullReferenceException nullEx) { }
                catch (Exception Inex) { insertCondition = false; }
                return insertCondition && insertEtape;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> SelectAllGroupeValidation()
        {
            try
            {
                Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> Datas = new Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>();
                List<CsGroupeValidation> lsGrp = Entities.GetEntityListFromQuery<CsGroupeValidation>(ParamProcedure.LISTE_WKF_GROUPES_VALIDATION());
                lsGrp.ForEach((CsGroupeValidation grp) =>
                {
                    List<CsRHabilitationGrouveValidation> lsHabil = Entities.GetEntityListFromQuery<CsRHabilitationGrouveValidation>(ParamProcedure.LISTE_WKF_HABILITATION_GROUPE_VALIDATION(grp.PK_ID));
                    Datas.Add(grp, lsHabil);
                });
                return Datas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public static bool CheckIfConditionIsRespected<T>(string Condition, T table, ref string msgErr, ref bool isOk)
        //    where T : class, new()
        //{
        //    bool isRespected = true;
        //    //On a l'objet et le type de l'objet, on va tester donc la condition
        //    string[] tokens = Condition.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        //    //normalement on a trois tokens
        //    if (tokens.Count() == 3)
        //    {
        //        string columnName = tokens[0]; //Nom de la colonne;
        //        OPERATEUR op = EnumerationString.GetOperateurEnum(tokens[1]); //l'Operateur
        //        string value = tokens[2];   //La valeur

        //        //Maintenant on compile notre If
        //        Type propType = null;
        //        object currentValue = RetourneValueFromClasse<T>(table, columnName, ref propType);
        //        if (propType.IsGenericType && propType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        //        {
        //            //Si le type est nullable alors, on récupère la destination
        //            //Par exemple si c'est Nullable<int>, la destination est int
        //            propType = Nullable.GetUnderlyingType(propType);
        //        }
        //        var correctValue = null != currentValue ? Convert.ChangeType(currentValue, propType) : null;
        //        var conditionCorrectValue = "null" != value.ToLower() ? Convert.ChangeType(GetValueFromString(value), propType) : null;

        //        //Bon sincèrement il n'ya pas de plusieurs type de données dans la base généralement
        //        //ce qui est sûr on teste 
        //        switch (op)
        //        {
        //            case OPERATEUR.Equals:
        //                isRespected = (correctValue.ToString() == conditionCorrectValue.ToString());
        //                break;
        //            case OPERATEUR.Different:
        //                {
        //                    isRespected = (correctValue.ToString() != conditionCorrectValue.ToString());
        //                    break;
        //                }
        //            case OPERATEUR.GreaterOrEquals:
        //            case OPERATEUR.GreatherThan:
        //            case OPERATEUR.LessOrEquals:
        //            case OPERATEUR.LessThan:
        //                {
        //                    //peut importe, le type, on utilise double qui est plus grand que
        //                    //int, decimal
        //                    double dbValueLine = double.Parse(correctValue.ToString());
        //                    double dbValueCondition = double.Parse(conditionCorrectValue.ToString());

        //                    if (op == OPERATEUR.GreatherThan) isRespected = dbValueLine > dbValueCondition ? true : false;
        //                    else if (op == OPERATEUR.GreaterOrEquals) isRespected = dbValueLine >= dbValueCondition ? true : false;
        //                    else if (op == OPERATEUR.LessThan) isRespected = dbValueLine < dbValueCondition ? true : false;
        //                    else if (op == OPERATEUR.LessOrEquals) isRespected = dbValueLine <= dbValueCondition ? true : false;
        //                };
        //                break;
        //            default:
        //                isRespected = true;
        //                break;
        //        }

        //        //Disposition des var
        //        if (null != currentValue) GC.SuppressFinalize(currentValue);
        //        if (null != correctValue) GC.SuppressFinalize(correctValue);
        //        if (null != conditionCorrectValue) GC.SuppressFinalize(conditionCorrectValue);
        //        if (null != propType) GC.SuppressFinalize(propType);

        //        isOk = true;
        //    }

        //    else
        //    {
        //        msgErr = "La valeur n'existe pas dans la base de données";
        //        isOk = false;
        //        isRespected = false;
        //    }

        //    return isRespected;
        //}

        public Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> SelectAllAffectationEtapeWorkflow(Guid pRWKF)
        {
            try
            {
                var lsEtapesAffWKF = Entities.GetEntityListFromQuery<CsRAffectationEtapeWorkflow>(ParamProcedure.LISTE_WKF_ETAPESWORFKLOW(pRWKF));
                //Ensuite les conditions s'il en existe
                Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement> TheDatas = new Dictionary<CsRAffectationEtapeWorkflow, CsConditionBranchement>();
                foreach (CsRAffectationEtapeWorkflow aff in lsEtapesAffWKF)
                {
                    galadbEntities context = new galadbEntities();
                    var theGroupeValidation = context.GROUPE_VALIDATION.Where(grp => grp.PK_ID == aff.FK_IDGROUPEVALIDATIOIN)
                        .FirstOrDefault();
                    if (null != theGroupeValidation) aff.GROUPEVALIDATION = theGroupeValidation.GROUPENAME;

                    var lsConditions = Entities.GetEntityListFromQuery<CsConditionBranchement>(ParamProcedure.LISTE_WKF_CONDITION(aff.PK_ID));
                    if (0 != lsConditions.Count)
                    {
                        CsConditionBranchement lCondition = lsConditions.FirstOrDefault();
                        aff.CONDITION = lCondition.COLONNENAME + " " + lCondition.OPERATEUR + " " + lCondition.VALUE;

                        var Step = context.ETAPE.Where(e => e.PK_ID == lCondition.FK_IDETAPEVRAIE.Value)
                            .FirstOrDefault();
                        if (null != Step) aff.ETAPECONDITIONVRAIE = Step.NOM;
                    }
                    TheDatas.Add(aff, lsConditions.FirstOrDefault());

                    context.Dispose();
                    GC.SuppressFinalize(context);
                }
                return TheDatas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
