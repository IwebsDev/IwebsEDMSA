using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galatee.Security
{
   public static class Connexion
    {
        private const string Identity = "GalaUser";
        private const string Password = "passgalatee";

        public static string GalaUserId
        {
            get { return Identity; }
        }

        public static string GalaUserPassword
        {
            get { return Password; }
        }
    }
}
