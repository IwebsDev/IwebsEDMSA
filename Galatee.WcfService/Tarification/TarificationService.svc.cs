using Galatee.DataAccess;
using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace WcfService.Tarification
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TarificationService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TarificationService.svc or TarificationService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TarificationService : ITarificationService
    {
        
        #region Redevence

        public List<CsTypeRedevence> LoadAllTypeRedevance()
        {
            List<CsTypeRedevence> ListeTypeRedevance = new List<CsTypeRedevence>();
            try
            {
                ListeTypeRedevance = new DbTarification().LoadAllTypeRedevance();
                return ListeTypeRedevance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsTypeLienRedevence> LoadAllTypeLienRedevance()
        {
            List<CsTypeLienRedevence> ListeTypeLienRedevance = new List<CsTypeLienRedevence>();
            try
            {
                ListeTypeLienRedevance = new DbTarification().ListeTypeLienRedevance();
                return ListeTypeLienRedevance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsTypeLienProduit> LoadAllTypeLienProduit()
        {
            List<CsTypeLienProduit> ListeTypeLienProduit= new List<CsTypeLienProduit>();
            try
            {
                ListeTypeLienProduit = new DbTarification().ListeTypeLienProduit();
                return ListeTypeLienProduit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsRedevance> LoadAllRedevance()
        {
            List<CsRedevance> ListeLotScelle = new List<CsRedevance>();
            try
            {
                //ListeLotScelle = new DbTarification().LoadAllRedevance();
                ListeLotScelle = new DbFacturation().LoadAllRedevance();
                return ListeLotScelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int SaveRedevance(List<CsRedevance> ListeRedevanceToUpdate, List<CsRedevance> ListeRedevanceToInserte, List<CsRedevance> ListeRedevanceToDelete)
        {
            List<CsRedevance> ListeRedevanceToUpdate_ = (List<CsRedevance>)ListeRedevanceToUpdate;
            List<CsRedevance> ListeRedevanceToInserte_ = (List<CsRedevance>)ListeRedevanceToInserte;
            List<CsRedevance> ListeRedevanceToDelete_ = (List<CsRedevance>)ListeRedevanceToDelete;

            try
            {
                return new DbTarification().SaveRedevance(ListeRedevanceToUpdate_, ListeRedevanceToInserte_, ListeRedevanceToDelete_);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return true;
        }

        public bool CheickCodeRedevanceExist(string Code)
        {
            try
            {
                return new DbTarification().CheickCodeRedevanceExist(Code);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }


        #endregion 

        #region RechercheTarif

        public List<CsContenantCritereTarif> LoadAllContenantCritereTarif()
        {
            List<CsContenantCritereTarif> ListeContenantCritereTarif = new List<CsContenantCritereTarif>();
            try
            {
                ListeContenantCritereTarif = new DbTarification().LoadAllContenantCritereTarif();
                return ListeContenantCritereTarif;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsRechercheTarif> LoadAllRechercheTarif()
        {
            List<CsRechercheTarif> ListeRechercheTarif = new List<CsRechercheTarif>();
            try
            {
                ListeRechercheTarif = new DbTarification().LoadAllRechercheTarif();
                return ListeRechercheTarif;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int SaveRechercheTarif(List<CsRechercheTarif> ListeRedevanceToUpdate, List<CsRechercheTarif> ListeRedevanceToInserte, List<CsRechercheTarif> ListeRedevanceToDelete)
        {
            List<CsRechercheTarif> ListeRedevanceToUpdate_ = (List<CsRechercheTarif>)ListeRedevanceToUpdate;
            List<CsRechercheTarif> ListeRedevanceToInserte_ = (List<CsRechercheTarif>)ListeRedevanceToInserte;
            List<CsRechercheTarif> ListeRedevanceToDelete_ = (List<CsRechercheTarif>)ListeRedevanceToDelete;

            try
            {
                return new DbTarification().SaveRechercheTarif(ListeRedevanceToUpdate_, ListeRedevanceToInserte_, ListeRedevanceToDelete_);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return true;
        }


        #endregion 

        #region Variable de tarificaton

        public List<CsVariableDeTarification> LoadAllVariableTarif()
        {
            List<CsVariableDeTarification> ListeVariableDeTarification = new List<CsVariableDeTarification>();
            try
            {
                ListeVariableDeTarification = new DbTarification().LoadAllVariableTarif ();
                return ListeVariableDeTarification;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsModeCalcul> LoadAllModeCalcule()
        {
            List<CsModeCalcul> ListeModeCalcul = new List<CsModeCalcul>();
            try
            {
                ListeModeCalcul = new DbTarification().LoadAllModeCalcule();
                return ListeModeCalcul;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsModeApplicationTarif> LoadAllModeApplicationTarif()
        {
            List<CsModeApplicationTarif> ListeModeApplicationTarif = new List<CsModeApplicationTarif>();
            try
            {
                ListeModeApplicationTarif = new DbTarification().LoadAllModeApplicationTarif();
                return ListeModeApplicationTarif;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveVariableTarif(List<CsVariableDeTarification> ListeRedevanceToUpdate, List<CsVariableDeTarification> ListeRedevanceToInserte, List<CsVariableDeTarification> ListeRedevanceToDelete)
        {
            List<CsVariableDeTarification> ListeRedevanceToUpdate_ = (List<CsVariableDeTarification>)ListeRedevanceToUpdate;
            List<CsVariableDeTarification> ListeRedevanceToInserte_ = (List<CsVariableDeTarification>)ListeRedevanceToInserte;
            List<CsVariableDeTarification> ListeRedevanceToDelete_ = (List<CsVariableDeTarification>)ListeRedevanceToDelete;

            try
            {
                return new DbTarification().SaveVariableTarif(ListeRedevanceToUpdate_, ListeRedevanceToInserte_, ListeRedevanceToDelete_);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return true;
        }

        #endregion

        #region TarifFacturation

        //Charger liste des Tarif Facturation
        public List<CsTarifFacturation> LoadAllTarifFacturation(int? idCentre, int? idProduit, int? idRedevance, int? idCodeRecherche)
        {
            
            List<CsTarifFacturation> ListeTarifFacturation = new List<CsTarifFacturation>();
            try
            {
                ListeTarifFacturation = new DbTarification().LoadAllTarifFacturation(idCentre,idProduit ,idRedevance ,idCodeRecherche );
                return ListeTarifFacturation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsDetailTarifFacturation> LoadAllDetailTarifFacturation()
        {

            List<CsDetailTarifFacturation> ListeDetailTarifFacturation = new List<CsDetailTarifFacturation>();
            try
            {
                ListeDetailTarifFacturation = new DbTarification().LoadAllDetailTarifFacturation();
                return ListeDetailTarifFacturation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SaveTarifFacturation(CsTarifFacturation Tarification ,int Action)
        {
            try
            {
                return new DbTarification().SaveTarifFacturation(Tarification, Action);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return true;
        }
        public bool CreateTarif(List<CsTarifFacturation> Tarification)
        {
            try
            {
                return new DbTarification().CreateTarif(Tarification);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return true;
        }
        //public int SaveTarifFacturation(List<CsTarifFacturation> ListeRedevanceToUpdate, List<CsTarifFacturation> ListeRedevanceToInserte, List<CsTarifFacturation> ListeRedevanceToDelete)
        //{
        //    List<CsTarifFacturation> ListeRedevanceToUpdate_ = (List<CsTarifFacturation>)ListeRedevanceToUpdate;
        //    List<CsTarifFacturation> ListeRedevanceToInserte_ = (List<CsTarifFacturation>)ListeRedevanceToInserte;
        //    List<CsTarifFacturation> ListeRedevanceToDelete_ = (List<CsTarifFacturation>)ListeRedevanceToDelete;

        //    try
        //    {
        //        return new DbTarification().SaveTarifFacturation(ListeRedevanceToUpdate_, ListeRedevanceToInserte_, ListeRedevanceToDelete_);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //return true;
        //}
        public bool DuplicationTarifVersCentre(int AncienIdCentre, int NouveauIdCentre, int? produit)
        {
            try
            {
                return new DbTarification().DuplicationTarifVersCentre( AncienIdCentre,  NouveauIdCentre,produit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       public List<CsUniteComptage> LoadAllUniteComptage()
        {
            List<CsUniteComptage> ListeTarifFacturation = new List<CsUniteComptage>();
            try
            {
                ListeTarifFacturation = new DbTarification().LoadAllUniteComptage();
                return ListeTarifFacturation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Generer Ctarcomp
            //Génération de lignes des Tarification en fonction du code recherche 
        public List<CsTarifFacturation> LoadTarifGenerer(string FK_IDRECHERCHETARIF, string FK_IDVARIABLETARIF,string Produit)
            {
                List<CsTarifFacturation> ListeTarifFacturation = new List<CsTarifFacturation>();
                try
                {
                    ListeTarifFacturation = new DbTarification().LoadTarifGenerer(FK_IDRECHERCHETARIF, FK_IDVARIABLETARIF, Produit);
                    return ListeTarifFacturation;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        #endregion
    }
}
