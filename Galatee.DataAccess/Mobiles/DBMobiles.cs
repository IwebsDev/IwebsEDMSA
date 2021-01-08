using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.DataAccess;
using Galatee.Structure;
//using Microsoft.Reporting;
//using Microsoft.Reporting.WebForms;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Web.Mail;
using System.ServiceModel.Activation;
using Galatee.Entity.Model;
using System.Threading.Tasks;

namespace Galatee.DataAccess
{
    public class DBMobiles
    {
        public DBMobiles()
        {

        }

        public static List<CsMobile> ObtenirDonneeParamtrageMobile(List<CsMobile> mobiles)
        {
            try
            {
                List<CsMobile> mbiles = new List<CsMobile>();
                 Parallel.ForEach(
                           mobiles, m =>
                           {
                               //m.InitData.Releveur = MobileProcedures.RetourneDonneesReleveurEtTournee(m.AdresseMac);
                           });
                 //DataTable casIndex = MobileProcedures.RetourneToutCasIndex();
                 //mobiles.ForEach(m => m.InitData.CasIndex = casIndex);

                 return mobiles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
    }

}




