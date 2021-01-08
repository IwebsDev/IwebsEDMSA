using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Galatee.Structure;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Threading;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class TAB300Access
    {

         string Langue = Thread.CurrentThread.CurrentUICulture.Name;
        private string ConnectionString;
        private SqlCommand cmd = null;
        private SqlConnection cn = null;
        SqlTransaction transaction = null;

        //Enumere enumer = new Enumere();


        public TAB300Access()
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



        public  string SelectLibelleFonction(string codeFonction)
        {
                string fonction = string.Empty;

                //Objet cn
                cn = new SqlConnection(ConnectionString);
                //Objet Command
                cmd = new SqlCommand("SPX_TA_SelFonction", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000;

                SqlParameter param = cmd.Parameters.Add(new SqlParameter("@CodeFonction", codeFonction));
                param.Direction = ParameterDirection.Input;

                try
                {

                //Ouverture
                cn.Open();

                //Object datareader
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();

                    throw new Exception("Code fonction inconnu.");
                }
                while (reader.Read())
                {
                    if (codeFonction == (reader.GetValue(0).ToString().Trim()).Substring(3, 3))
                    {
                        fonction = reader.GetValue(1).ToString().Trim();
                        break;
                    }
                }
                //Fermeture reader
                reader.Close();
                return fonction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //Fermeture base
                cn.Close();
            }


        }

        public string SelectLibelleProduit(int IdProduit)
        {
            try
            {
                var produit = Entities.GetEntityFromQuery<aBanque>(ParamProcedure.PARAM_PRODUIT_RETOURNEById(IdProduit));
                return produit != null ? produit.LIBELLE : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public  string SelectLibelleCentre(string codeCentre)
        {
                string centre = string.Empty;

                //Objet cn
                cn = new SqlConnection(ConnectionString);
                //Objet Command
                cmd = new SqlCommand("SPX_Directeur_SelCentre", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = cmd.Parameters.Add(new SqlParameter("@CodeCentre", codeCentre));
                param.Direction = ParameterDirection.Input;

                try
                {

                //Ouverture
                cn.Open();

                //Object datareader
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();
                    cn.Close();

                    throw new Exception("Code centre inconnu.");
                }
                while (reader.Read())
                {
                    if (codeCentre == reader.GetValue(0).ToString().Trim())
                    {
                        centre = reader.GetValue(1).ToString().Trim();
                        break;
                    }
                }
                //Fermeture reader
                reader.Close();

                return centre;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //Fermeture base
                cn.Close();
            }

        }

    }
}
