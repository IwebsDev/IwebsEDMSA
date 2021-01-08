using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galatee.Entity.Model.Rpnt;

namespace Galatee.DataAccess.Rpnt
{
    public class DBCAMPAGNE
    {
        //#region Methode de detection BTA
        public List<Galatee.Structure.Rpnt.CsRefMethodesDeDetectionClientsBTA> GetMethodeDetectionBTA()
        {
            try
            {
                //List<CsREFMETHODEDEDETECTIONCLIENTSBTA> MTHDETEC_BAT = new List<CsREFMETHODEDEDETECTIONCLIENTSBTA>();
                //foreach (var item in new RpntProcedures().GetMethodeDetectionBTA())
                //{
                //    MTHDETEC_BAT.Add(new CsREFMETHODEDEDETECTIONCLIENTSBTA { DESCRIPTION = item.DESCRIPTION, LIBELE_METHODE = item.LIBELE_METHODE, METHODE_ID = item.METHODE_ID });
                //}
                //return MTHDETEC_BAT;
                return new RpntProcedures().GetMethodeDetectionBTA();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //    public bool SaveMethodeDetectionBTA(CsREFMETHODEDEDETECTIONCLIENTSBTA MthDetection, bool IsUpdate)
        //    {
        //        try
        //        {
        //            return new RpntProcedures().SaveMethodeDetectionBTA(MthDetection, IsUpdate);
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }
        //    }
        //    public bool DeleteMethodeDetectionBTA(int Methode_Id)
        //    {
        //        try
        //        {
        //            return new RpntProcedures().DeleteMethodeDetectionBTA(Methode_Id);
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }
        //    }
            

        //#endregion

        //#region Methode de detection HTA
        //    public List<CsREFMETHODEDEDETECTIONCLIENTSHTA> GetMethodeDetectionHTA()
        //    {
        //        try
        //        {
        //            return new RpntProcedures().GetMethodeDetectionHTA();
        //        }
        //        catch (Exception ex )
        //        {
                
        //            throw ex ;
        //        }
        //    }

        //#endregion

        //#region Paramettre methode de detection
        //    public List<CsTBPARAMETRE> LoadParametreMthDtect()
        //    {
        //        try
        //        {
        //            return new RpntProcedures().LoadParametreMthDtect();
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }

        //    }
        //    public bool SaveParamtreMethodeBTA(CsTBPARAMETRE paramtre, bool IsUpdate)
        //    {
        //        try
        //        {
        //            return new RpntProcedures().SaveParamtreMethodeBTA(paramtre, IsUpdate);
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }
        //    }
        //    public bool DeleteParamMethodeDetectionBTA(int param_id)
        //    {
        //        try
        //        {
        //            return new RpntProcedures().DeleteParamMethodeDetectionBTA(param_id);
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }
        //    }
        //#endregion

        //#region Campagne

        //    public List<CsCentre> GetExploitationByUO(CsREFUNITEORGANISATIONNELLE UO)
        //    {
        //        try
        //        {
        //            return new RpntProcedures().GetExploitationByUO(UO);
        //        }
        //        catch (Exception ex )
        //        {
                
        //            throw ex;
        //        }
        //    }
        public bool InsertCampagneBTA(CsCampagnesBTAAccessiblesParLUO CampBAT)
        {
            try
            {
                return new RpntProcedures().InsertCampagneBTA(CampBAT);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<CsCampagnesBTAAccessiblesParLUO> GetCampagneBTAControle()
        {
            try
            {
                return new RpntProcedures().GetCampagneBTAControle();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //#endregion



        public List<CsBrt> GetBranchementBTAControle(List<CsClient> ClientSelection)
        {
            try
            {
                return new RpntProcedures().GetBranchementBTAControle(ClientSelection);
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
                return new RpntProcedures().GetTypeClient();
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
                return new RpntProcedures().GetTypeTarif();
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
                return new RpntProcedures().GetGroupeFacture();
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
                return new RpntProcedures().GetAgentZont();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsClient> GetClientEligible(string CodeMethode, int FkidCentre, string CodeTypeClient, string CodeTypeTarif, string CodeTypeCompteur, string CodeAgentZone, string CodeGroupe, string nombremois, string txt_comparaison_periode1, string txt_comparaison_periode2, string txt_Code_Cas, double Nombre_Recurence, double pourcentage)
        {
            try
            {
                return new RpntProcedures().GetClientEligible(CodeMethode, FkidCentre, CodeTypeClient, CodeTypeTarif, CodeTypeCompteur, CodeAgentZone, CodeGroupe, nombremois, txt_comparaison_periode1, txt_comparaison_periode2, txt_Code_Cas, Nombre_Recurence, pourcentage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<List<CsProduit>> LoadProduit()
        //{
        //    try
        //    {
        //        return new RpntProcedures().LoadProduit();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        //public bool DeleteBranchement(int brt_id)
        //{
        //    try
        //    {
        //        return new RpntProcedures().DeleteBranchement(brt_id);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public List<CsEvenement> GetConsoInfo(string client, string periode, string nbr_cycle_fact)
        //{
        //    try
        //    {
        //        return new RpntProcedures().GetConsoInfo(client, periode, nbr_cycle_fact);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public CsClient GetClientInfo(string client)
        //{
        //    try
        //    {
        //        return new RpntProcedures().GetClientInfo(client);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public CsAbon GetContratInfo(string brt)
        //{
        //    try
        //    {
        //        return new RpntProcedures().GetContratInfo(brt);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public CsCanalisation GetCanalisationInfo(string brt)
        //{
        //    try
        //    {
        //        return new RpntProcedures().GetCanalisationInfo(brt);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public List<CsTcompteur> GetTypeCompteur()
        {
            try
            {
                return new RpntProcedures().GetTypeCompteur();
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
                return new RpntProcedures().GetMois();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        ////public bool AffecteBranchementACampagne(CsTBCAMPAGNECONTROLEBTA campagne, List<CsBrt> branchement_eligible, CsREFMETHODEDEDETECTIONCLIENTSBTA Mth_Detection, string PeriodeDepart)
        ////{
        ////    try
        ////    {
        ////        //return new RpntProcedures().AffecteBranchementACampagne( campagne, branchement_eligible,  Mth_Detection,  PeriodeDepart);
        ////    }
        ////    catch (Exception ex)
        ////    {

        ////        throw ex;
        ////    }
        ////}

        public bool SaveCampagneElement(List<CsCampagnesBTAAccessiblesParLUO> ListeCampBAT)
        {
            try
            {

                return new RpntProcedures().SaveCampagneElement(ListeCampBAT);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public List<CsTournee> GetTournee()
        //{
        //    try
        //    {

        //        return new RpntProcedures().GetTournee();
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

        //        return new RpntProcedures().GetFamilleAnomalie();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //public bool SaveAnomalies(CsTBELEMENTLOTDECONTROLEBTA ELEMENTLOTDECONTROLEBTA, List<CsREFFAMILLEANOMALIEBTA> REFFAMILLEANOMALIEBTA, bool estisole)
        //{
        //    try
        //    {
        //        return new RpntProcedures().SaveAnomalies( ELEMENTLOTDECONTROLEBTA,  REFFAMILLEANOMALIEBTA,estisole);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<CsREFFAMILLEANOMALIEBTA> GetAnomalies(int BRT_ID, Guid? LOT_ID)
        //{
        //    try
        //    {
        //        return new RpntProcedures().GetAnomalies( BRT_ID,  LOT_ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<CsTBCONSTATFRAUDEBTA> GetConstat(int BRT_ID, Guid? LOT_ID, Guid? CAMPAGNE_ID)
        //{
        //    try
        //    {
        //        return new RpntProcedures().GetConstat(BRT_ID, LOT_ID, CAMPAGNE_ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<CsBrt> GetBranchement()
        //{
        //    try
        //    {
        //        return new RpntProcedures().GetBranchement();
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
        //        return new RpntProcedures().GetconstatFraud(code_centre, txt_Branchement);
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
        //        return new RpntProcedures().GetBranchementByCritere(code_centre, txt_ref_Cli, txt_Nom_Cli);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<CsTBCONSTATFRAUDEBTA> GetAllConstat(int BRT_ID, Guid? LOT_ID)
        //{
        //    try
        //    {
        //        return new RpntProcedures().GetAllConstat(BRT_ID, LOT_ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<CsREFABAQUE_CHOIXTC> LoadAllAbaques()
        //{
        //    try
        //    {
        //        return new RpntProcedures().LoadAllAbaques();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public List<CsREFNIVEAUDETENSIONHTA> LoadAllNiveauTension()
        //{
        //    try
        //    {
        //        return new RpntProcedures().LoadAllNiveauTension();
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
        //        return new RpntProcedures().loadAllTypComptage();
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
        //        return new RpntProcedures().LoadPlageDePuissance();
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
        //        return new RpntProcedures().SaveAllAbaques(ListeAbaques);
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
        //        return new RpntProcedures().Stat_LoadConstatparFamille(debut, fin, codecentre, codefamille, codetype);
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
        //        return new RpntProcedures().Stat_LoadCampgne(debut, fin, codecentre);
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
        //        return new RpntProcedures().LoadConstatCampagne(TBCAMPAGNECONTROLEBTA);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion

        public List<CsClient> GetClienteBTADuLotControle(Structure.Rpnt.CstbLotsDeControleBTA Lot)
        {
             try
             {
                 return new RpntProcedures().GetClienteBTADuLotControle(Lot);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
        }

        public bool InitialisationFraud(List<Galatee.Structure.Rpnt.CstbElementsLotDeControleBTA> ListElementLot)
        {
            try
            {
                return new RpntProcedures().InitialisationFraud(ListElementLot);
                //return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
