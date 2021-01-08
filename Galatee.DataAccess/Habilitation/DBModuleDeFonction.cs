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
    public class DBModuleDeFonction
    {

        public DBModuleDeFonction()
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





        ////bool DeleteHabilitationProfil(string codeFonction,SqlCommand cmd)
        ////{
        ////    int rowsAffected = -1;

        ////        cmd.Parameters.Clear();
        ////        cmd.CommandTimeout = 240;
        ////        cmd.CommandType = CommandType.StoredProcedure;
        ////        cmd.CommandText = "SPX_ADM_SUPPRIME_HABILITATION_PROFIL";
        ////        try
        ////        {
        ////            cmd.Parameters.Add("@CodeFonction", SqlDbType.VarChar).Value = codeFonction;
        ////            DBBase.SetDBNullParametre(cmd.Parameters);

        ////            rowsAffected = cmd.ExecuteNonQuery();
        ////            return Convert.ToBoolean(rowsAffected);

        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            Transaction.RollBackTransaction(cmd);
        ////            throw ex;
        ////        }
        ////}

        public bool InsertionModuleDeFonction(List<CsModuleDeFonction> modules)
        {
                try
                    {

                        List<Galatee.Entity.Model.MODULESDEFONCTION> dt = Entities.ConvertObject<Galatee.Entity.Model.MODULESDEFONCTION, CsModuleDeFonction>(modules);
                        return Galatee.Entity.Model.AdminProcedures.InsertionModuleDeFonction(dt);
                   }  
                  catch (Exception ex)
                    {
                        throw ex;
                    }
        }

        private void ObtenirIdFonction(string fonction, List<CsModule> modules)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    FONCTION _fonction = context.FONCTION.FirstOrDefault(f => f.CODE == fonction);
                    //modules.ForEach(h => h.PK_ID = _fonction.PK_ID);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsModuleDeFonction> SelectModuleDeFonctionByFonction( string fonction)
        {
            try
            {
                //DataTable obj = Galatee.Entity.Model.AuthentProcedures.SelectHabilitationByUser(Iduser);
                //List<CsModuleDeFonction> l = Tools.Utility.GetEntityFromQuery<CsHabilitationProgram>(obj).ToList();
                return null ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    

    }
}
