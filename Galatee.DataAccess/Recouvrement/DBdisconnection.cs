using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Galatee.Structure;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Xml;

namespace Galatee.DataAccess
{
    public class DBdisconnection
    {
        private string ConnectionString;
        private SqlCommand cmd = null;
        private SqlConnection cn = null;
        SqlTransaction transaction = null;

        public DBdisconnection()
        {
            try
            {
                //ConnectionString = GalateeConfig.ConnectionStrings[Enumere.ConnexionGALADB].ConnectionString;
                ConnectionString = Session.GetSqlConnexionString();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
  

    }
}
