

#region using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Galatee.DataAccess;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections;
using System.Diagnostics;
using Galatee.Structure;
using Galatee.Entity.Model;
using System.Linq;
#endregion

namespace Galatee.DataAccess
{

    /// <summary>
    ///	This class is the base repository for the CRUD operations on the ObjDEVIS objects.
    /// </summary>
    public class DBDEVIS : DBBase
    {
    

        public static List<ObjDEVIS> GetByCodeAppareilFromAPPAREILSDEVIS(Int32 pCodeAppareil)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_RETOURNEByCodeAppareilFromAPPAREILSDEVIS(pCodeAppareil));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static List<ObjDEVIS> GetAllDevis()
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static List<ObjDEVIS> GetByCentre(String centre)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_RETOURNEByCentre(centre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public static CsDemandeBase GetDevisByNumDemande(string  pNumDemande)
        {
            CsDemandeBase demande = new CsDemandeBase();
            try
            {
                return Entities.GetEntityFromQuery<CsDemandeBase>(AccueilProcedures.GetDemandeByNumDemande(pNumDemande));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
 

        public static CsDemandeBase   GetDevisByNumIdDevis(int pIdDevis)
        {
            CsDemandeBase demande = new CsDemandeBase();
            try
            {
                return new DBAccueil().GetDemandeByNumIdDemande(pIdDevis);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsDemandeBase> GetDevisByNumEtape(int NumEtape)
        {
            List<CsDemandeBase> Listresult = new List<CsDemandeBase>();
            try
            {
                List<CsDemandeBase> ListDevis = Entities.GetEntityListFromQuery<CsDemandeBase>(DevisProcedures.DEVIS_DEVIS_RETOURNEByNumEtape(NumEtape));
                return ListDevis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static double CalculerDelaiDevis(DateTime? DateCreation, int? pDelaisExecution)
        {
            DateTime date, currentDate;
            currentDate = DateTime.Now.Date;
            TimeSpan difference;
            double day;
            try
            {
                if (DateCreation != null)
                {
                    date = (DateTime)DateCreation;
                    if (pDelaisExecution != null)
                    {
                        day = (double)pDelaisExecution;
                        difference = (currentDate.Date - date.Date);
                        var delai = day - difference.TotalDays;
                        return delai;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static DateTime? CalculerDateLimite(DateTime? DateCreation, int? pDelaisExecution)
        {
            DateTime date, currentDate;
            currentDate = DateTime.Now.Date;
            double day;
            try
            {
                if (DateCreation != null)
                {
                    date = (DateTime)DateCreation;
                    if (pDelaisExecution != null)
                    {
                        day = (double)pDelaisExecution;
                        var dateLimite = date.AddDays(day);
                        return dateLimite;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjDEVIS> GetByIdEtapeDevis(int idEtapeDevis)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_RETOURNEByIdEtapeDevis(idEtapeDevis));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static List<ObjDEVIS> GetByIdPieceIdentite(int idPieceIdentite)
        {
            try
            {
                return null;
                //return Entities.GetEntityListFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_RETOURNEByIdPieceIdentite(idPieceIdentite));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static List<ObjDEVIS> GetByCodeProduit(int codeProduit)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_RETOURNEByCodeProduit(codeProduit));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static List<ObjDEVIS> GetBySite(int site)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_RETOURNEBySite(site));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static List<ObjDEVIS> GetByIdTypeDevis(Int32 idTypeDevis)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_RETOURNEByIdTypeDevis(idTypeDevis));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static List<ObjDEVIS> GetByIsAnalysed(Boolean isAnalysed)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_RETOURNEByIsAnalysed(isAnalysed));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static List<ObjDEVIS> GetByCodeProduitIdTypeDevis(int codeProduit, int idTypeDevis)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_RETOURNEByCodeProduitIdTypeDevis(codeProduit, idTypeDevis));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static ObjDEVIS GetByNumDevis(string numDevis)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_RETOURNEByNumDevis(numDevis));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ObjDEVIS GetByIdDevis(int pIdDevis)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_RETOURNEById(pIdDevis));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<ObjDEVIS> ChargerDevisAEncaisser()
        {
            List<ObjDEVIS> Listresult = new List<ObjDEVIS>();
            try
            {
                List<ObjDEVIS> ListDevis = Entities.GetEntityListFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_EditionDevisAEncCaisser());
                foreach (ObjDEVIS devis in ListDevis)
                {
                    devis.DATELIMITE = CalculerDateLimite(devis.DATEDECREATION, devis.DELAIEXECUTIONETAPE);
                    devis.DELAI = CalculerDelaiDevis(devis.DATEDECREATION, devis.DELAIEXECUTIONETAPE);
                    Listresult.Add(devis);
                }
                return Listresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjDEVIS> ChargerDevisBanchementAFinaliser()
        {
            List<ObjDEVIS> Listresult = new List<ObjDEVIS>();
            try
            {
                List<ObjDEVIS> ListDevis = Entities.GetEntityListFromQuery<ObjDEVIS>(DevisProcedures.DEVIS_DEVIS_EditionDevisBanchementAFinaliser());
                foreach (ObjDEVIS devis in ListDevis)
                {
                    devis.DATELIMITE = CalculerDateLimite(devis.DATEDECREATION, devis.DELAIEXECUTIONETAPE);
                    devis.DELAI = CalculerDelaiDevis(devis.DATEDECREATION, devis.DELAIEXECUTIONETAPE);
                    Listresult.Add(devis);
                }
                return Listresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Sylla
        public static List<CsTypeDOCUMENTSCANNE> ChargerTypeDocument()
        {
            try
            {
                return Entities.GetEntityListFromQuery< CsTypeDOCUMENTSCANNE>(DevisProcedures.ChargerTypeDocument());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Structure.CsCATEGORIECLIENT_TYPECLIENT> ChargerCategorieClient_TypeClient()
        {
            try
            {
                return Entities.GetEntityListFromQuery<Galatee.Structure.CsCATEGORIECLIENT_TYPECLIENT>(DevisProcedures.ChargerCategorieClient_TypeClient());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Structure.CsCATEGORIECLIENT_USAGE> ChargerCategorieClient_Usage()
        {
            try
            {
                return Entities.GetEntityListFromQuery<Galatee.Structure.CsCATEGORIECLIENT_USAGE>(DevisProcedures.ChargerCategorieClient_Usage());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Structure.CsNATURECLIENT_TYPECLIENT> ChargerNatureClient_TypeClient()
        {
            try
            {
                return Entities.GetEntityListFromQuery<Galatee.Structure.CsNATURECLIENT_TYPECLIENT>(DevisProcedures.ChargerNatureClient_TypeClient());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Structure.CsUSAGE_NATURECLIENT> ChargerUsage_NatureClient()
        {
            try
            {
                return Entities.GetEntityListFromQuery<Galatee.Structure.CsUSAGE_NATURECLIENT>(DevisProcedures.ChargerUsage_NatureClient());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region CR TRAVAUX

        public List<CsOrganeScellable> LoadListeOrganeScellable(int FK_IDTDEM,int FK_IDPRODUIT)
        {
            try
            {
                return Entities.GetEntityListFromQuery<Galatee.Structure.CsOrganeScellable>(DevisProcedures.LoadListeOrganeScellable(FK_IDTDEM, FK_IDPRODUIT));
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Structure.CsScelle> LoadListeScelle()
        {
            try
            {
                return Entities.GetEntityListFromQuery<Structure.CsScelle>(DevisProcedures.LoadListeScelle());
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Structure.CsScelle> LoadListeScelle(int idAgentMatricule, int fk_TypeDemande)
        {
            try
            {
                return Entities.GetEntityListFromQuery<Structure.CsScelle>(DevisProcedures.LoadListeScelle(idAgentMatricule, fk_TypeDemande));
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Structure.CsScelle> LoadListeScelle(int idAgentMatricule, int fk_TypeDemande,int Activie_ID)
        {
            try
            {
                return Entities.GetEntityListFromQuery<Structure.CsScelle>(DevisProcedures.LoadListeScelle(idAgentMatricule, fk_TypeDemande, Activie_ID));
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
} 
