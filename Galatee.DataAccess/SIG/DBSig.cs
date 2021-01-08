using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Galatee.Entity.Model.Reports;
using Galatee.Structure;

namespace Galatee.DataAccess
{
    public class DBSig
    {
        private string ConnectionString;
        private SqlConnection sqlConnection;

        public DBSig()
        {
            //try
            //{
            //    ConnectionString = Session.GetSqlConnexionString();
            //    sqlConnection = new SqlConnection(ConnectionString);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public List<CsAbonneCarte> RetournerCoodonnesAbonne(CsClientRechercher client)
        {
            DataTable dt = SIGProcedures.RetournerCoodonnesAbonne(client);
            return Galatee.Entity.Model.Entities.GetEntityListFromQuery<CsAbonneCarte>(dt);
        }

        public List<CsAbonneCarte> RetourneCoordonneesAbonCampagne(string Idcoupure)
        {
            DataTable dt = SIGProcedures.RetournerCoodonnesAbonne(Idcoupure);
            List<CsAbonneCarte> _lstClientLocalise = Galatee.Entity.Model.Entities.GetEntityListFromQuery<CsAbonneCarte>(dt);
            if (_lstClientLocalise != null)
                return _lstClientLocalise.Where(t => !string.IsNullOrEmpty(t.Longitude) && !string.IsNullOrEmpty(t.Latitude)).ToList();
            else
                return new List<CsAbonneCarte>();

        }
    }
}
