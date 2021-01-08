using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    /// <summary>
    /// DB_DIACOMP
    /// </summary>
    public class DB_REGLAGECOMPTEUR : Galatee.DataAccess.Parametrage.DbBase
    {
        public List<CsReglageCompteur > SelectAllReglageCompteur()
        {
            try
            {
                /*Stephen 13-02-2019*/
                //return Entities.GetEntityListFromQuery<CsReglageCompteur>(ParamProcedure.PARAM_REGLAGECOMPTEUR_RETOURNE());
                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsReglageCompteur> _LstItem = new List<CsReglageCompteur>();
                _LstItem = db.RetourneReglageCompteur();
                return _LstItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsReglageCompteur> SelectAllReglageCompteurByCentre(int pId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsReglageCompteur>(ParamProcedure.PARAM_REGLAGECOMPTEUR_RETOURNEByCentre(pId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsReglageCompteur> SelectAllReglageCompteurByCentreIdProduitId(int pProduitId, int pCentreId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsReglageCompteur>(ParamProcedure.PARAM_REGLAGECOMPTEUR_RETOURNEByIdProduitIdCentreId(pProduitId, pCentreId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsReglageCompteur> SelectAllReglageCompteurByProduit(int pProduitId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsReglageCompteur>(ParamProcedure.PARAM_REGLAGECOMPTEUR_RETOURNEByIdProduit(pProduitId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsReglageCompteur pDiacomp)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.REGLAGECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.REGLAGECOMPTEUR, CsReglageCompteur>(pDiacomp));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsReglageCompteur> pDiacompCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.REGLAGECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.REGLAGECOMPTEUR, CsReglageCompteur>(pDiacompCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsReglageCompteur pDiacomp)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.REGLAGECOMPTEUR  >(Entities.ConvertObject<Galatee.Entity.Model.REGLAGECOMPTEUR  , CsReglageCompteur>(pDiacomp));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsReglageCompteur> pDiacompCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.REGLAGECOMPTEUR  >(Entities.ConvertObject<Galatee.Entity.Model.REGLAGECOMPTEUR  , CsReglageCompteur>(pDiacompCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsReglageCompteur pDiacomp)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.REGLAGECOMPTEUR  >(Entities.ConvertObject<Galatee.Entity.Model.REGLAGECOMPTEUR  , CsReglageCompteur>(pDiacomp));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsReglageCompteur> pDiacompCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.REGLAGECOMPTEUR  >(Entities.ConvertObject<Galatee.Entity.Model.REGLAGECOMPTEUR  , CsReglageCompteur>(pDiacompCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
