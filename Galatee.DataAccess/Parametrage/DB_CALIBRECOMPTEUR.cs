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
    public class DB_CALIBRECOMPTEUR : Galatee.DataAccess.Parametrage.DbBase
    {

        public List<CsCalibreCompteur> SelectAllCalibreCompteurByCentre(int pId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCalibreCompteur>(ParamProcedure.PARAM_CALIBRECOMPTEUR_RETOURNEByCentre (pId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCalibreCompteur> SelectAllCalibreCompteurByCentreIdProduitId(int pProduitId, int pCentreId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCalibreCompteur>(ParamProcedure.PARAM_CALIBRECOMPTEUR_RETOURNEByIdProduitIdCentreId (pProduitId, pCentreId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCalibreCompteur> SelectAllCalibreCompteurByProduit(int pProduitId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCalibreCompteur>(ParamProcedure.PARAM_CALIBRECOMPTEUR_RETOURNEByIdProduit (pProduitId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsCalibreCompteur pDiacomp)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.CALIBRECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.CALIBRECOMPTEUR, CsCalibreCompteur>(pDiacomp));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsCalibreCompteur> pDiacompCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.CALIBRECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.CALIBRECOMPTEUR, CsCalibreCompteur>(pDiacompCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsCalibreCompteur pDiacomp)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CALIBRECOMPTEUR  >(Entities.ConvertObject<Galatee.Entity.Model.CALIBRECOMPTEUR  , CsCalibreCompteur>(pDiacomp));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsCalibreCompteur> pDiacompCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CALIBRECOMPTEUR  >(Entities.ConvertObject<Galatee.Entity.Model.CALIBRECOMPTEUR  , CsCalibreCompteur>(pDiacompCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsCalibreCompteur pDiacomp)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CALIBRECOMPTEUR  >(Entities.ConvertObject<Galatee.Entity.Model.CALIBRECOMPTEUR  , CsCalibreCompteur>(pDiacomp));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsCalibreCompteur> pDiacompCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CALIBRECOMPTEUR  >(Entities.ConvertObject<Galatee.Entity.Model.CALIBRECOMPTEUR   , CsCalibreCompteur>(pDiacompCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    #region ADO .Net from Entity : Stephen 26-01-2019

        public List<CsCalibreCompteur> SelectAllCalibreCompteur()
        {
            try
            {
                //return Entities.GetEntityListFromQuery<CsCalibreCompteur>(ParamProcedure.PARAM_CALIBRECOMPTEUR_RETOURNE());
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("CALIBRECOMPTEUR");
                //return Entities.GetEntityListFromQuery<CsCalibreCompteur>(dt);

                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsCalibreCompteur> _LstItem = new List<CsCalibreCompteur>();
                _LstItem = db.RetourneCalibreCompteur();
                return _LstItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    #endregion


    }
}
