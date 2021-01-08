using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Galatee.Structure;
using System.Data;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public  class CommonDAL
    {
        /// </summary>
        private   string ConnectionString;

        private   SqlConnection cn = null;
        /// <summary>
        /// _Transaction
        /// </summary>
        private   bool _Transaction;
        /// <summary>
        /// Transaction
        /// </summary>
        public   bool Transaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }

        }

        private   SqlCommand cmd = null;
        public CommonDAL()
        {
            //ConnectionString = Session.GetSqlConnexionString();
            
        }
        public List<CsNature> RetourneAllNature()
        {
            CsNature _ta;
            List<CsNature> ListeTA = new List<CsNature>();
            ConnectionString = Session.GetSqlConnexionStringAbo07();
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectNATURE;

            try
            {

                return new List<CsNature>();
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectNATURE + ":" + ex.Message);
            }
            finally
            {
                if (this.cn.State == ConnectionState.Open)
                    this.cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        /// <summary>
        /// Ramene tous les sites et centres inclus qui définissent le périmetre d'action
        /// du matricule connecté
        /// </summary>
        /// <param name="centreCible">Centre dans lequel l'operation impactera</param>
        /// <param name="matricule">Matricule de l'utilisateur qui initie l'operation</param>
        /// <returns></returns>
        bool ValiderDonnees(List<CsLibelle> donnees,string centreCible)
          {
              bool action = false;
            try 
	            {	      
  
		         foreach(CsLibelle perimetre in donnees)
                 {
                     if (centreCible == perimetre.fk_Centre)
                  {
                      action=  true;
                      break;
                  }
                 }
                return action;
	            }
	            catch (Exception)
	            {
		          return false;
	            }
          }
        private List<CsLibelle> Fill(SqlDataReader reader, List<CsLibelle> rows, int start, int pageLength)
          {
              for (int i = 0; i < start; i++)
              {
                  if (!reader.Read())
                      return rows;
              }

              for (int i = 0; i < pageLength; i++)
              {
                  if (!reader.Read())
                      break;

                  CsLibelle c = new CsLibelle();
                  c.fk_Site = (Convert.IsDBNull(reader["Site"])) ? string.Empty : (System.String)reader["Site"];
                  c.fk_Centre = (Convert.IsDBNull(reader["Centre"])) ? string.Empty : (System.String)reader["Centre"];
                  rows.Add(c);
              }
              return rows;
          }

        public List<CsCategorieClient> RetourneCategorieClient()
        {
            try 
	        {
	            DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneCategorieClient();
                List<CsCategorieClient> c = Entities.GetEntityListFromQuery<CsCategorieClient>(dt).ToList();
                return c;

		     }
	        catch (Exception ex)
	        {
		
		        throw ex;
	        }

            
        }

        public static List<CsProduitFacture> RetourneTousProduit()
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousProduit();
                List<CsProduitFacture> c = Entities.GetEntityListFromQuery<CsProduitFacture>(dt).ToList();
                return c;             
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static CsProduitFacture RetourneUnProduit(int idProd)
        //{
        //    CsProduitFacture produit=from prod in RetourneTousProduit()
        //                             where prod.pk

        //    return 
        //}


    #region ADO .Net from Entity : Stephen 26-01-2019
        public List<CsCentre> GetCentre()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousCentres();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("CENTRE");
                List<CsCentre> c = Entities.GetEntityListFromQuery<CsCentre>(dt).ToList();
                return c;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    #endregion




    }
}
