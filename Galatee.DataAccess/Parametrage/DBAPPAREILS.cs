
#region Using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using Galatee.Structure;
using System.Data.SqlClient;
using Galatee.Entity.Model;
#endregion

namespace Galatee.DataAccess
{
    ///<summary>
    /// This class is the Data Access Logic Component implementation for the <see cref="APPAREILS"/> business entity.
    ///</summary>
    [DataObject]
    public partial class DBAPPAREILS /*: DBBase*/
    {
        public static ObjAPPAREILS GetByCodeAppareil(int pCodeAppareil)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjAPPAREILS>(ParamProcedure.PARAM_APPAREILS_RETOURNEByCodeAppareil(pCodeAppareil));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjAPPAREILS> GetAll()
        {
            try
            {
                //return Entities.GetEntityListFromQuery<ObjAPPAREILS>(ParamProcedure.PARAM_APPAREILS_RETOURNE());
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("APPAREILS");
                return Entities.GetEntityListFromQuery<ObjAPPAREILS>(dt);

            }
            catch (Exception ex)
            {
              throw ex;
            }

        }

        public bool Insert(ObjAPPAREILS pAppareil)
        {
            try
            {
               return Entities.InsertEntity<Galatee.Entity.Model.APPAREILS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILS, ObjAPPAREILS>(pAppareil));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Insert(List<ObjAPPAREILS> pAppareilCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.APPAREILS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILS, ObjAPPAREILS>(pAppareilCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(ObjAPPAREILS pAppareil)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.APPAREILS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILS, ObjAPPAREILS>(pAppareil));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool Update(List<ObjAPPAREILS> pAppareilCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.APPAREILS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILS, ObjAPPAREILS>(pAppareilCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(ObjAPPAREILS pAppareil)
        {

            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.APPAREILS>(Entities.ConvertObject<Galatee.Entity.Model.APPAREILS, ObjAPPAREILS>(pAppareil));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  List<ObjAPPAREILSDEVIS> GetAppareilByDevis(string NumDevis)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjAPPAREILSDEVIS >(ParamProcedure.PARAM_APPAREILS_RETOURNE_BYDEVIS(NumDevis));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


       
    }
}