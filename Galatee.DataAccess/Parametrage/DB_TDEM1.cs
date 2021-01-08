using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
     public class DB_TDEM1 : Galatee.DataAccess.Parametrage.DbBase
    {
        public List<CsTdem> SelectAllTdem()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsTdem>(CommonProcedures.RetourneTousTDEM());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
