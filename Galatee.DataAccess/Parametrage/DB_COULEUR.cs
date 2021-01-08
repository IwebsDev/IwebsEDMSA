using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public static class DB_COULEUR  
    {


        public static List<CsCouleurScelle> SelectAllCouleur()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCouleurScelle>(ParamProcedure.PARAM_COULEUR_RETOURNE ());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Delete(CsCouleurScelle pMarqueModel)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.RefCouleurlot >(Entities.ConvertObject<Galatee.Entity.Model.RefCouleurlot, CsCouleurScelle>(pMarqueModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Delete(List<CsCouleurScelle> pMarqueModel)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.RefCouleurlot>(Entities.ConvertObject<Galatee.Entity.Model.RefCouleurlot, CsCouleurScelle>(pMarqueModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(CsCouleurScelle pMarqueModel)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.RefCouleurlot>(Entities.ConvertObject<Galatee.Entity.Model.RefCouleurlot, CsCouleurScelle>(pMarqueModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(List<CsCouleurScelle> pTMarqueModelCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.RefCouleurlot>(Entities.ConvertObject<Galatee.Entity.Model.RefCouleurlot, CsCouleurScelle>(pTMarqueModelCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Insert(CsCouleurScelle pTcompt)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.RefCouleurlot>(Entities.ConvertObject<Galatee.Entity.Model.RefCouleurlot, CsCouleurScelle>(pTcompt));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Insert(List<CsCouleurScelle> pTcomptCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.RefCouleurlot>(Entities.ConvertObject<Galatee.Entity.Model.RefCouleurlot, CsCouleurScelle>(pTcomptCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
