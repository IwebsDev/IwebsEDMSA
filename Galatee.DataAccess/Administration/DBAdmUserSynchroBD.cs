using Galatee.Entity.Model;
using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.DataAccess
{
    public class DBAdmUserSynchroBD
    {

        public List<Structure.CsAgent> LoadAgentBaseDistantez(string Requette, string Provider, string DataSource, string IniialeCatalog, string UserId, string Password)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AdminProceduresSynchroBD.LoadAgentBaseDistantez(Requette, Provider, DataSource, IniialeCatalog, UserId, Password);
                List<CsAgent> agentremotedb =Entities.GetEntityListFromQuery<CsAgent>(dt);
                DataTable dt1 = Galatee.Entity.Model.AdminProcedures.ChargeListeDesAgents();
                List<CsAgent> agentlocaldb = Entities.GetEntityListFromQuery<CsAgent>(dt1);

                List<CsAgent> agentremotedbInactif = agentremotedb.Where(a => a.ACTIF == false)!=null?agentremotedb.Where(a => a.ACTIF == false).ToList():new List<CsAgent>();
               
                var matriculagentremotedbInactif=agentremotedbInactif.Select(ag=>ag.MATRICULE).ToList();

                List<CsAgent> agentlocaldb_a_desactiver = agentlocaldb.Where(a => matriculagentremotedbInactif.Contains(a.MATRICULE)).ToList();
                var matriculeagentlocaldb=agentlocaldb.Select(ag=>ag.MATRICULE).ToList();
                List<CsAgent> nouveauagenttosynchro = agentremotedb.Where(a => !(matriculeagentlocaldb.Contains(a.MATRICULE))).ToList();

                foreach (var item in agentlocaldb_a_desactiver)
                {
                    item.ACTIF = false;
                }
                nouveauagenttosynchro.AddRange(agentlocaldb_a_desactiver);
                return nouveauagenttosynchro;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<string> LoadAgentBaseDistante(string Requette, string Provider, string DataSource, string IniialeCatalog, string UserId, string Password)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.AdminProceduresSynchroBD.LoadAgentBaseDistantez(Requette, Provider, DataSource, IniialeCatalog, UserId, Password);

                //List<CsAgent> agentremotedb = Entities.GetEntityListFromQuery<CsAgent>(dt);
                //DataTable dt1 = Galatee.Entity.Model.AdminProcedures.ChargeListeDesAgents();
                //List<CsAgent> agentlocaldb = Entities.GetEntityListFromQuery<CsAgent>(dt1);

                //List<CsAgent> agentremotedbInactif = agentremotedb.Where(a => a.ACTIF == false) != null ? agentremotedb.Where(a => a.ACTIF == false).ToList() : new List<CsAgent>();

                //var matriculagentremotedbInactif = agentremotedbInactif.Select(ag => ag.MATRICULE).ToList();

                //List<CsAgent> agentlocaldb_a_desactiver = agentlocaldb.Where(a => matriculagentremotedbInactif.Contains(a.MATRICULE)).ToList();
                //var matriculeagentlocaldb = agentlocaldb.Select(ag => ag.MATRICULE).ToList();
                //List<CsAgent> nouveauagenttosynchro = agentremotedb.Where(a => !(matriculeagentlocaldb.Contains(a.MATRICULE))).ToList();

                //foreach (var item in agentlocaldb_a_desactiver)
                //{
                //    item.ACTIF = false;
                //}
                //nouveauagenttosynchro.AddRange(agentlocaldb_a_desactiver);


                return Entities.DataTableToCsvFormat(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveAgent(List<CsAgent> agenttosynchro, string Requette)
        {
            try
            {
                bool result = Galatee.Entity.Model.AdminProceduresSynchroBD.SaveAgent(agenttosynchro, Requette);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string LoadRequest()
        {
            try
            {
                string result = Galatee.Entity.Model.AdminProceduresSynchroBD.LoadRequest();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetProviderList()
        {
            try
            {
                List<string> result = Galatee.Entity.Model.AdminProceduresSynchroBD.GetProviderList();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<string> GetSQLDatabaseList(string serverInstanceName, bool useWindowsAuthentication, string username, string password)
        {
            try
            {
                List<string> result = Galatee.Entity.Model.AdminProceduresSynchroBD.GetSQLDatabaseList( serverInstanceName,  useWindowsAuthentication,  username,  password);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool TestBaseDistante(string Requette, string Provider, string DataSource, string IniialeCatalog, string UserId, string Password)
        {
            try
            {
                bool dt = Galatee.Entity.Model.AdminProceduresSynchroBD.TestBaseDistante(Requette, Provider, DataSource, IniialeCatalog, UserId, Password);
                
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
