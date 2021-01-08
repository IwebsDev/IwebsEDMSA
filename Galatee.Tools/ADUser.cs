using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galatee.Tools
{
    /// <summary>
    /// Classe représentant un utilisateur Active Directory
    /// </summary>
    public class ADUser
    {
        public string Email { get; set; }
        
        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string Groupe { get; set; }
        
    }
}
