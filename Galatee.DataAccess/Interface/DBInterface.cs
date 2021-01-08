using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections;
using Inova.Tools.Utilities;
//
using Galatee.Structure  ;
using System.Globalization;
using Galatee.Entity.Model;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;


namespace Galatee.DataAccess
{
    public class DBInterface
    {
        public DBInterface()
        {
            try
            {
                //ConnectionString = GalateeConfig.ConnectionStrings[Enumere.ConnexionGALADB].ConnectionString;
                //ConnectionString = Session.GetSqlConnexionString();
            }
            catch (Exception)
            {
                
                throw;
            }

        }
        //public static bool EnvoieDonneesClientAEclipse(CsClient client, CsAbon abon, CsAg ag, CsBrt brt, CsCompteur compteur)
        //{
        //    try
        //    {
        //        return InterfaceProcedures.EnvoieDonneesClientAEclipse(client, abon, ag, brt, compteur, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public static bool EnvoieDonneesClientAEclipseAP_DP(CsAbon abon, CsCompteur Nouveaucompteur, CsCompteur Aciencompteur, bool ChangementCompteur)
        //{
        //    try
        //    {
        //        return InterfaceProcedures.EnvoieDonneesClientAEclipseAP_DP(abon, Nouveaucompteur, Aciencompteur, ChangementCompteur);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public static bool EnvoieDonneesClientAEclipseResiliation(CsAbon abon, CsCompteur Nouveaucompteur, CsCompteur Aciencompteur, bool ChangementCompteur)
        //{
        //    try
        //    {
        //        return InterfaceProcedures.EnvoieDonneesClientAEclipseResiliation(abon, Nouveaucompteur, Aciencompteur, ChangementCompteur);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public static bool EnvoieDonneesClientAEclipseMutation(CsClient Nouveauclient, CsClient Ancienclient, CsAbon abon, CsAg ag, CsBrt brt, CsCompteur compteur, bool Modification)
        //{
        //    try
        //    {
        //        return InterfaceProcedures.EnvoieDonneesClientAEclipseMutation(Nouveauclient, Ancienclient, abon, ag, brt, compteur, Modification);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}


