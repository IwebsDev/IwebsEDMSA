using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Galatee.DataAccess
{
    public static class Global
    {


        //static string _ChaineConnexionServerDB = @"Data Source=" + ServerName + "; Initial Catalog=" + Catalog +
        //                    "; User ID=" + UserDB + "; Password=" + UserPwd + ";";
        //public static string ChaineConnexionServerDB
        //{
        //    get { return Global._ChaineConnexionServerDB; }
        //    set { Global._ChaineConnexionServerDB = value; }
        //}

        private static string LocalDbName = "TANGO.sdf";

        private static string AssemblyExecPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;

        private static int index = AssemblyExecPath.LastIndexOf('\\');

        private static string _ChaineConnexionLocalDb = string.Format("Data Source={0}", System.IO.Path.Combine(HostingEnvironment.MapPath("~/App_Data/"), LocalDbName));
        public static string ChaineConnexionLocalDb
        {
            get { return Global._ChaineConnexionLocalDb; }
            set { Global._ChaineConnexionLocalDb = value; }
        }

        //public static bool ChangeDbConnexion(string pServer, string pCatalog, string pUserDb, string pPwd)
        //{
        //    try
        //    {
        //        _ServerName = pServer;
        //        _UserDB = pUserDb;
        //        _UserPwd = pPwd;
        //        _Catalog = pCatalog;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



    }
}
