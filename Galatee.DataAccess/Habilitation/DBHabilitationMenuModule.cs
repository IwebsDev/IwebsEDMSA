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
    public class DBHabilitationMenuModule
    {
        public DBHabilitationMenuModule()
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
        public List<CSMenuGalatee> ChargerMenuEtSousMenu(string Module, int idprofil)
        {
            try
            {
                DataTable objet = Galatee.Entity.Model.AuthentProcedures.ListeMenuSousMenu(Module, idprofil);
                return Galatee.Tools.Utility.GetEntityFromQuery<CSMenuGalatee>(objet).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CSMenuGalatee> ChargerMenuEtSousMenuDuProfil( string  module)
        {

            try
            {
                return Tools.Utility.GetEntityFromQuery<CSMenuGalatee>(Galatee.Entity.Model.AuthentProcedures.ListeMenuParHabilitationProfil(module)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ObtenirIdFonction(string fonction, List<CsHabilitationProgram> habilitations)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    FONCTION _fonction = context.FONCTION.FirstOrDefault(f => f.CODE == fonction);
                    habilitations.ForEach(h => h.FK_IDFONCTION = _fonction.PK_ID);
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private IList<Galatee.Structure.CSMenuGalatee> RechercherFils(int _menuId, IList<Galatee.Structure.CSMenuGalatee> ListeDeMenu)
        {
            if (ListeDeMenu == null || ListeDeMenu.Count == 0)
                return null;

            //- Récuparation des sous-menu du menu principal fourni en paramètre
            IList<Galatee.Structure.CSMenuGalatee> permissions = ListeDeMenu.Where(perm => perm.MainMenuID == _menuId).OrderBy(q => q.MenuOrder).ToList();
            return permissions;
        }
    }
  }


