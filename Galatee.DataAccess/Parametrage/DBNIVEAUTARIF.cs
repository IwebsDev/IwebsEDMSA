using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBNIVEAUTARIF
    {

        public List<CsNiveauTarif> SelectAllNiveauTarif()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsNiveauTarif>(ParamProcedure.PARAM_NIVEAUTARIF_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsNiveauTarif pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.NIVEAUTARIF>(Entities.ConvertObject<Galatee.Entity.Model.NIVEAUTARIF, CsNiveauTarif>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsNiveauTarif> pEntity)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.NIVEAUTARIF>(Entities.ConvertObject<Galatee.Entity.Model.NIVEAUTARIF, CsNiveauTarif>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsNiveauTarif pEntity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.NIVEAUTARIF>(Entities.ConvertObject<Galatee.Entity.Model.NIVEAUTARIF, CsNiveauTarif>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsNiveauTarif> pEntityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.NIVEAUTARIF>(Entities.ConvertObject<Galatee.Entity.Model.NIVEAUTARIF, CsNiveauTarif>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsNiveauTarif pEntity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.NIVEAUTARIF>(Entities.ConvertObject<Galatee.Entity.Model.NIVEAUTARIF, CsNiveauTarif>(pEntity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsNiveauTarif> pEntityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.NIVEAUTARIF>(Entities.ConvertObject<Galatee.Entity.Model.NIVEAUTARIF, CsNiveauTarif>(pEntityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
