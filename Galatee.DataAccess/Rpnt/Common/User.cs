using System;
using System.Collections.Generic;
using System.Text;

namespace Galatee.DataAccess.Common
{
    [Serializable]
    public class User
    {
        public  User()
        {
        }
 
        public string Matricule { get; set; }
        public string DisplayedName { get; set; }
        //public string Secteur { get; set; }
        public string CodeUniteOrganisationnelle { get; set;  }
        public string LibelleUniteOrganisationnelle { get; set; }
        //public string Direction { get; set; }
        public string RoleName { get; set; }
        public Guid RoleID { get; set; }
        
        public override string ToString()
        {
            return this.DisplayedName;
        }

    }
}
