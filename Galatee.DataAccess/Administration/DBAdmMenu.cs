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
    public class DBAdmMenu : Galatee.DataAccess.Parametrage.DbBase
    {
        
        public DBAdmMenu()
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

        public List<CsAdmMenu> GetAll()
        {
            try
            {
                DataTable objet = Galatee.Entity.Model.AdminProcedures.RetourneListeToutModuleFonction();
                return Galatee.Tools.Utility.GetEntityFromQuery<CsAdmMenu>(objet).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsAdmMenu> GetAllModuleFonction()
        {
            try
            {
                List<CsAdmMenu> lesModule = new List<CsAdmMenu>();
                galadbEntities context = new galadbEntities();
                List<Galatee.Entity.Model.ADMMENU> dt = AdminProcedures.RetourneListeToutMenuProfilTotal(context);
                foreach (Galatee.Entity.Model.ADMMENU item in dt)
                {
                   CsAdmMenu leModule = new CsAdmMenu();
                   leModule = Entities.ConvertObject<CsAdmMenu, Galatee.Entity.Model.ADMMENU>(item);
                   leModule.lstMenuProfil = Entities.ConvertObject<CsMenuDuProfil , Galatee.Entity.Model.MENUSDUPROFIL >(item.MENUSDUPROFIL.ToList() );
                   lesModule.Add(leModule);
                }
                context.Dispose();
                return lesModule;
                }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsAdmMenu admProfilUsers)
        {
            try
            {

                Galatee.Entity.Model.ADMMENU admProfilUser = Galatee.Tools.Utility.ConvertEntity<Galatee.Entity.Model.ADMMENU, CsAdmMenu>(admProfilUsers);
                using (galadbEntities context = new galadbEntities())
                {
                    return Entities.InsertEntity<Galatee.Entity.Model.ADMMENU>(admProfilUser);

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
