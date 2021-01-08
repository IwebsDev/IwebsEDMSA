using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
//
using System.Configuration;
using System.Data;
//using GestionChequeImpaye.DataAcess;
//using GestionChequeImpaye.Common;
//using GestionChequeImpaye.Core;

namespace Galatee.DataAccess
{
    public class DBBase
    {
        /// <summary>
        /// Converti les valeurs null en DBNull
        /// </summary>
        /// <param name="parameters">La liste des parametres de la requete</param>
        public static void SetDBNullParametre(SqlParameterCollection parameters)
        {
            foreach (SqlParameter Parameter in parameters)
            {
                if (Parameter.Value == null || Parameter.Value.ToString() == string.Empty)
                {
                    Parameter.Value = DBNull.Value;
                }
            }
        }

        //public SqlConnection ConnectionClient(string Centre, string Client, string Ordre)
        //{
        //    DBSITE db = new DBSITE();
        //    SqlConnection sqlCnct = null;
        //    string conncetionstring = string.Empty;
        //    SITECollection lstSite = db.GetByCENTREFromDIRECTEUR(Centre);

        //    foreach (SITE site in lstSite)
        //    {
        //        conncetionstring = Session.GetSqlConnexionString(site);
        //        DBLClient dbc = new DBLClient();
        //        sqlCnct = new SqlConnection(conncetionstring);
        //        if (sqlCnct.State != System.Data.ConnectionState.Open)
        //            sqlCnct.Open();
        //        if (dbc.VerifExistantceClient(sqlCnct, Centre, Client, Ordre))
        //        {
        //            if (sqlCnct.State != System.Data.ConnectionState.Open)
        //                sqlCnct.Open();
        //            return sqlCnct;
        //        }
        //    }
        //    throw new Exception("Abonné inextant dans GALATEE (" + Centre + Client + Ordre + ")");
        //    //return null;
        //}

        public static string RetourneStringListeObject(List<int> p)
        {
            string Idc = string.Empty;
            int pass = 0;
            foreach (int item in p)
            {
                if (pass == 0)
                    Idc = item.ToString();
                else
                    Idc = Idc + "," + item;
                pass++;
            }
            return Idc;
        }
        public static string RetourneStringListeObject(List<string> p)
        {
            string Idc = string.Empty;
            int pass = 0;
            if (p!=null)
            {
                foreach (string item in p)
                {
                    if (pass == 0)
                        Idc = item.ToString();
                    else
                        Idc = Idc + "," + item;
                    pass++;
                } 
            }
            return Idc;
        }
        public static SqlCommand InitTransaction(string ConnectionString)
        {
            SqlCommand laCommande = new SqlCommand();
            laCommande.Connection = new SqlConnection(ConnectionString);

            if (laCommande.Connection.State == ConnectionState.Closed)
                laCommande.Connection.Open();

            SqlTransaction trans = laCommande.Connection.BeginTransaction();
            laCommande.Transaction = trans;

            return laCommande;
        }

        public static SqlConnection  InitConnection(string ConnectionString)
        {
            SqlConnection laConnection = new SqlConnection();
            laConnection = new SqlConnection(ConnectionString);

            if (laConnection.State == ConnectionState.Closed)
                laConnection.Open();


            return laConnection;
        }
    }
}
