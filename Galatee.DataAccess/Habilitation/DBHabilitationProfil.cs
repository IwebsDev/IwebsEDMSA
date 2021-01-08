using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using System.Collections;
using Inova.Tools.Utilities;
//
using Galatee.Structure;
using System.Reflection;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBHabilitationProfil
    {
        public DBHabilitationProfil()
        {
            try
            {
                //ConnectionString = Session.GetSqlConnexionString();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private string ConnectionString;
        private SqlCommand cmd = null;
        private SqlConnection cn = null;


        public bool InsertionHabilitationProfil(List<CsProfil> modules)
        {
            try
            {

                List<Galatee.Entity.Model.PROFIL> dt = Entities.ConvertObject<Galatee.Entity.Model.PROFIL, CsProfil>(modules);
                return Galatee.Entity.Model.AdminProcedures.InsertionHabilitationProfil(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
