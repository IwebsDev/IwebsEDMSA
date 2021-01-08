
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
    public partial class DBTYPEDETAXE /*: Galatee.DataAccess.Parametrage.DbBase*/
	{
 

        

        public  List<CsTypeTaxe > GetAll()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsTypeTaxe>(ParamProcedure.PARAM_TYPETAXE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsTypeTaxe pTypeTaxe)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPETAXE>(Entities.ConvertObject<Galatee.Entity.Model.TYPETAXE, CsTypeTaxe>(pTypeTaxe));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsTypeTaxe> pTypeTaxeCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPETAXE>(Entities.ConvertObject<Galatee.Entity.Model.TYPETAXE, CsTypeTaxe>(pTypeTaxeCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsTypeTaxe pTypeTaxe)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPETAXE>(Entities.ConvertObject<Galatee.Entity.Model.TYPETAXE, CsTypeTaxe>(pTypeTaxe));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsTypeTaxe> pTypeTaxeCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPETAXE>(Entities.ConvertObject<Galatee.Entity.Model.TYPETAXE, CsTypeTaxe>(pTypeTaxeCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsTypeTaxe pTypeTaxe)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPETAXE>(Entities.ConvertObject<Galatee.Entity.Model.TYPETAXE, CsTypeTaxe>(pTypeTaxe));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsTypeTaxe> pTypeTaxeCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPETAXE>(Entities.ConvertObject<Galatee.Entity.Model.TYPETAXE, CsTypeTaxe>(pTypeTaxeCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	}
}