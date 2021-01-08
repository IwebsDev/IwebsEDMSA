
/*********************************************************************************
GENERER LE : 13/02/2010 20:02:57
*********************************************************************************/



#region Using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.ComponentModel;
using Galatee.Structure;


using System.Data.SqlClient;
using Galatee.Entity.Model;

#endregion

namespace Galatee.DataAccess
{

	///<summary>
	/// This class is the Data Access Logic Component implementation for the <see cref="TYPEDEVIS"/> business entity.
	///</summary>
	[DataObject]
    public partial class DBTYPECOMPTAGE /*: Galatee.DataAccess.Parametrage.DbBase*/
	{
        public  List<CsTypeComptage > GetAll()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsTypeComptage>(ParamProcedure.PARAM_TYPECOMPTAGE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsTypeComptage pTypeComptage)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPECOMPTAGE>(Entities.ConvertObject<Galatee.Entity.Model.TYPECOMPTAGE, CsTypeComptage>(pTypeComptage));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsTypeComptage> pTypeComptageCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPECOMPTAGE>(Entities.ConvertObject<Galatee.Entity.Model.TYPECOMPTAGE, CsTypeComptage>(pTypeComptageCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsTypeComptage pTypeComptage)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPECOMPTAGE>(Entities.ConvertObject<Galatee.Entity.Model.TYPECOMPTAGE, CsTypeComptage>(pTypeComptage));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsTypeComptage> pTypeComptageCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPECOMPTAGE>(Entities.ConvertObject<Galatee.Entity.Model.TYPECOMPTAGE, CsTypeComptage>(pTypeComptageCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsTypeComptage pTypeComptage)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPECOMPTAGE>(Entities.ConvertObject<Galatee.Entity.Model.TYPECOMPTAGE, CsTypeComptage>(pTypeComptage));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsTypeComptage> pTypeComptageCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPECOMPTAGE>(Entities.ConvertObject<Galatee.Entity.Model.TYPECOMPTAGE, CsTypeComptage>(pTypeComptageCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	}
}