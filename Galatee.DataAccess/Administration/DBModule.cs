using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using System.Transactions;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBModule : Galatee.DataAccess.Parametrage.DbBase
    
    {
        
        public DBModule()
        {
        }
        /// </summary>
        private string ConnectionString;
        private SqlConnection cn = null;
        /// <summary>
        /// _Transaction
        /// </summary>
        private bool _Transaction;
        /// <summary>
        /// Transaction
        /// </summary>
        public bool Transaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }

        }

        public List<CsModule> GetAll()
        {
            try
            {
                DataTable objet = Galatee.Entity.Model.AdminProcedures.RetourneListeToutModuleFonction();
                return Galatee.Tools.Utility.GetEntityFromQuery<CsModule>(objet).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsModule> GetAllModuleFonction()
        {
            try
            {
                List<CsModule> lesModule = new List<CsModule>();
                galadbEntities context = new galadbEntities();
                List<Galatee.Entity.Model.MODULE> dt = AdminProcedures.RetourneListeToutModuleFonctionTotal(context);
                foreach (Galatee.Entity.Model.MODULE item in dt)
                {
                   CsModule leModule = new CsModule();
                   leModule = Entities.ConvertObject<CsModule, Galatee.Entity.Model.MODULE>(item);
                   leModule.lstFonction = Entities.ConvertObject<CsModuleDeFonction , Galatee.Entity.Model.MODULESDEFONCTION >(item.MODULESDEFONCTION.ToList() );
                   lesModule.Add(leModule);
                }
                List<CsModule> lesModules = lesModule.OrderBy(g => g.PK_ID).ToList();
                context.Dispose();
                return lesModules;
                }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsModule admProfilUsers)
        {
            try
            {

                Galatee.Entity.Model.MODULE admProfilUser = Galatee.Tools.Utility.ConvertEntity<Galatee.Entity.Model.MODULE, CsModule>(admProfilUsers);
                using (galadbEntities context = new galadbEntities())
                {
                    return Entities.InsertEntity<Galatee.Entity.Model.MODULE>(admProfilUser);

                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SqlCommand cmd = null;
    }
    
}
