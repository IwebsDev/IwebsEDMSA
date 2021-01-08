using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public static class DB_ACTIVITECOULEUR  
    {


        public static List<CsCouleurActivite> SelectAllActiviteCouleur()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCouleurActivite>(ParamProcedure.PARAM_ACTIVITECOULEUR_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Delete(CsCouleurActivite pMarqueModel)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.ActiviteCouleur>(Entities.ConvertObject<Galatee.Entity.Model.ActiviteCouleur, CsCouleurActivite>(pMarqueModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Delete(List<CsCouleurActivite> pMarqueModel)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.ActiviteCouleur>(Entities.ConvertObject<Galatee.Entity.Model.ActiviteCouleur, CsCouleurActivite>(pMarqueModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(CsCouleurActivite pMarqueModel)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.ActiviteCouleur>(Entities.ConvertObject<Galatee.Entity.Model.ActiviteCouleur, CsCouleurActivite>(pMarqueModel));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(List<CsCouleurActivite> pTMarqueModelCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.ActiviteCouleur>(Entities.ConvertObject<Galatee.Entity.Model.ActiviteCouleur, CsCouleurActivite>(pTMarqueModelCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Insert(CsCouleurActivite pTcompt)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.ActiviteCouleur>(Entities.ConvertObject<Galatee.Entity.Model.ActiviteCouleur, CsCouleurActivite>(pTcompt));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Insert(List<CsCouleurActivite> pTcomptCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.ActiviteCouleur>(Entities.ConvertObject<Galatee.Entity.Model.ActiviteCouleur, CsCouleurActivite>(pTcomptCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
