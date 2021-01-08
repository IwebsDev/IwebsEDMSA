using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBRECHERCHETARIF 
    {

        public List<CsRechercheTarif> SelectAllRechercheTarif()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsRechercheTarif>(ParamProcedure.PARAM_RECHERCHETARIF_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsRechercheTarif pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.RECHERCHETARIF>(Entities.ConvertObject<Galatee.Entity.Model.RECHERCHETARIF, CsRechercheTarif>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsRechercheTarif> pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.RECHERCHETARIF>(Entities.ConvertObject<Galatee.Entity.Model.RECHERCHETARIF, CsRechercheTarif>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsRechercheTarif pEntity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.RECHERCHETARIF>(Entities.ConvertObject<Galatee.Entity.Model.RECHERCHETARIF, CsRechercheTarif>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsRechercheTarif> pEntityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.RECHERCHETARIF>(Entities.ConvertObject<Galatee.Entity.Model.RECHERCHETARIF, CsRechercheTarif>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsRechercheTarif pEntity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.RECHERCHETARIF>(Entities.ConvertObject<Galatee.Entity.Model.RECHERCHETARIF, CsRechercheTarif>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsRechercheTarif> pEntityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.RECHERCHETARIF>(Entities.ConvertObject<Galatee.Entity.Model.RECHERCHETARIF, CsRechercheTarif>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
