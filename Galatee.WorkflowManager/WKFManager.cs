using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Galatee.WorkflowManager
{
    public static class WKFManager
    {

        public static string SetErrorOnCodeDemande(string codeDemande)
        {
            return "ERREUR : " + codeDemande;
        }

        public static WKFSmtpInfo GetInfoServeurDeMail()
        {
            //TO DO : A implémenter prochainement
            return new WKFSmtpInfo();
        }

    }

    public class WKFSmtpInfo
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
    }
}