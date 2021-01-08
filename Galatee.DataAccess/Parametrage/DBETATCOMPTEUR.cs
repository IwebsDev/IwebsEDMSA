using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBETATCOMPTEUR
    {

        public List<CsEtatCompteur> SelectAllEtatComptage()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsEtatCompteur>(ParamProcedure.PARAM_ETATCOMPTEUR_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsEtatCompteur pEntity)
        {
            try
            {
                //return Entities.DeleteEntity<Galatee.Entity.Model.ETATCOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.ETATCOMPTEUR, CsEtatCompteur>(pEntity));
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsEtatCompteur> pEntity)
        {
            try
            {
                //return Entities.DeleteEntity<Galatee.Entity.Model.ETATCOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.ETATCOMPTEUR, CsEtatCompteur>(pEntity));
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsEtatCompteur pEntity)
        {
            try
            {
                //return Entities.UpdateEntity<Galatee.Entity.Model.ETATCOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.ETATCOMPTEUR, CsEtatCompteur>(pEntity));
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsEtatCompteur> pEntityCollection)
        {
            try
            {
                //return Entities.UpdateEntity<Galatee.Entity.Model.ETATCOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.ETATCOMPTEUR, CsEtatCompteur>(pEntityCollection));
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsEtatCompteur pEntity)
        {
            try
            {
                //return Entities.InsertEntity<Galatee.Entity.Model.ETATCOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.ETATCOMPTEUR, CsEtatCompteur>(pEntity));
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsEtatCompteur> pEntityCollection)
        {
            try
            {
                //return Entities.InsertEntity<Galatee.Entity.Model.ETATCOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.ETATCOMPTEUR, CsEtatCompteur>(pEntityCollection));
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
