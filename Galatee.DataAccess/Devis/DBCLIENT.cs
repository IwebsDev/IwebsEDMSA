
/*********************************************************************************
GENERER LE : 18/02/2010 00:54:04
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
//
using System.Data.SqlClient;
using Galatee.Entity.Model;
//

#endregion

namespace Galatee.DataAccess
{
	///<summary>
	/// This class is the Data Access Logic Component implementation for the <see cref="DEVISPIA"/> business entity.
	///</summary>
	[DataObject]
    public partial class DBCLIENT : Galatee.DataAccess.Parametrage.DbBase
	{
        /*
        public static List<cClient> Fill(IDataReader reader, List<cClient> rows, int start, int pageLength)
        {
            // advance to the starting row
            for (int i = 0; i < start; i++)
            {
                if (!reader.Read())
                    return rows; // not enough rows, just return
            }

            for (int i = 0; i < pageLength; i++)
            {
                if (!reader.Read())
                    break; // we are done

                cClient c = new cClient();
                c.CENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
                c.CLIENT = (Convert.IsDBNull(reader["CLIENT"])) ? string.Empty : (System.String)reader["CLIENT"];
                c.ORDRE = (Convert.IsDBNull(reader["ORDRE"])) ? string.Empty : (System.String)reader["ORDRE"];
                c.NOMABON = (Convert.IsDBNull(reader["NOMABON"])) ? string.Empty : (System.String)reader["NOMABON"];
                c.COMMUNE = (Convert.IsDBNull(reader["COMMUNE"])) ? string.Empty : (System.String)reader["COMMUNE"];
                c.QUARTIER = (Convert.IsDBNull(reader["QUARTIER"])) ? string.Empty : (System.String)reader["QUARTIER"];
                c.RUE = (Convert.IsDBNull(reader["RUE"])) ? string.Empty : (System.String)reader["RUE"];
                c.PORTE = (Convert.IsDBNull(reader["PORTE"])) ? string.Empty : (System.String)reader["PORTE"];
                c.TOURNEE = (Convert.IsDBNull(reader["TOURNEE"])) ? string.Empty : (System.String)reader["TOURNEE"];
                c.COMPTEUR = (Convert.IsDBNull(reader["COMPTEUR"])) ? string.Empty : (System.String)reader["COMPTEUR"];
                c.SOLDE = (Convert.IsDBNull(reader["SOLDE"])) ? 0 : (decimal)reader["SOLDE"];
                c.TELEPHONE = (Convert.IsDBNull(reader["TELEPHONE"])) ? string.Empty : (System.String)reader["TELEPHONE"];
                c.CNI = (Convert.IsDBNull(reader["CNI"])) ? string.Empty : (System.String)reader["CNI"];
                c.CATCLI = (Convert.IsDBNull(reader["CATCLI"])) ? string.Empty : (System.String)reader["CATCLI"];
                rows.Add(c);
            }
            return rows;
        }

        public List<cClient> SearchClient(string centre, string client, string nom, string produit)
        {
            string cns = Session.GetSqlConnexionString();
            SqlConnection connection = new SqlConnection(cns);
            SqlCommand command = new SqlCommand("SPX_DEVIS_CLIENT_RETOURNE_LISTByCentreClientNomProduit", connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlParameter param = command.Parameters.Add(new SqlParameter("@Centre", centre));
            param.Direction = ParameterDirection.Input;
            SqlParameter param0 = command.Parameters.Add(new SqlParameter("@Client", client));
            param0.Direction = ParameterDirection.Input;
            SqlParameter param1 = command.Parameters.Add(new SqlParameter("@Nom", nom));
            param1.Direction = ParameterDirection.Input;
            SqlParameter param2 = command.Parameters.Add(new SqlParameter("@Produit", produit));
            param2.Direction = ParameterDirection.Input;

            SetDBNullParametre(command.Parameters);

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<cClient> tmp = new List<cClient>();
                Fill(reader, tmp, 0, int.MaxValue);
                reader.Close();
                return tmp;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //Fermeture base
                if(connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Dispose();
                command.Dispose();
            }
        }

        public cClient GetClient(string centre, string client, string nom, string produit)
        {
            cClient row = new cClient();
            string cns = Session.GetSqlConnexionString();
            SqlConnection connection = new SqlConnection(cns);
            SqlCommand command = new SqlCommand("SPX_DEVIS_CLIENT_RETOURNEByCentreClientNomProduit", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlParameter param = command.Parameters.Add(new SqlParameter("@Centre", centre));
            param.Direction = ParameterDirection.Input;
            SqlParameter param0 = command.Parameters.Add(new SqlParameter("@Client", client));
            param0.Direction = ParameterDirection.Input;
            SqlParameter param1 = command.Parameters.Add(new SqlParameter("@Nom", nom));
            param1.Direction = ParameterDirection.Input;
            SqlParameter param2 = command.Parameters.Add(new SqlParameter("@Produit", produit));
            param2.Direction = ParameterDirection.Input;

            SetDBNullParametre(command.Parameters);

            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                List<cClient> tmp = new List<cClient>();
                Fill(reader, tmp, 0, int.MaxValue);
                reader.Close();

                if (tmp.Count == 1)
                {
                    row = tmp[0];
                }
                else if (tmp.Count == 0)
                {
                    row = null;
                }
                return row;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Dispose();
                command.Dispose();
            }
        }
        */

        public List<cClient> SearchClient( string pCentre, string pClient, string pNom, string pProduit)
        {
            List<cClient> lsClient = null;
            try
            {
                var result = Entities.GetEntityListFromQuery<cClient>(DevisProcedures.DEVIS_CLIENT_RETOURNE_ListByCentreClientNomProduit( pCentre,  pClient,  pNom,  pProduit));
                if (result != null && result.Count > 0)
                {
                    lsClient = new List<cClient>();
                    foreach (cClient item in result)
                    {
                        //item.SOLDE = FonctionCaisse.RetourneSoldeClient(item.fk.CENTRE, item.CLIENT, item.ORDRE);
                        lsClient.Add(item);
                    }
                }
                return lsClient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public cClient GetClient(string pCentre, string pClient, string pNom, string pProduit)
        {
            try
            {
                var client = Entities.GetEntityFromQuery<cClient>(DevisProcedures.DEVIS_CLIENT_RETOURNE_ListByCentreClientNomProduit(pCentre, pClient, pNom, pProduit));
                //if(client != null)
                    //client.SOLDE = FonctionCaisse.RetourneSoldeClient(client.CENTRE, client.CLIENT, client.ORDRE);
                return client;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	}
}