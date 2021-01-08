using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Galatee.Silverlight.Web.ServiceWeb;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Galatee.Silverlight.Web.serviceweb
{
    /// <summary>
    /// Description résumée de printingservice
    /// </summary>

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Pour autoriser l'appel de ce service Web depuis un script à l'aide d'ASP.NET AJAX, supprimez les marques de commentaire de la ligne suivante. 
    // [System.Web.Script.Services.ScriptService]
    public class printingservice : System.Web.Services.WebService
    {
        static Dictionary<string, List<object>> dicos = new Dictionary<string, List<object>>();

        [WebMethod]
        public CsReglement returnCSreglement()
        {

            try
            {
                return new CsReglement();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [WebMethod]
        public CsDetailMoratoire returnCsDetailMoratoire()
        {

            try
            {
                return new CsDetailMoratoire();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [WebMethod]
        public bool setDataInWebPart(string key, List<object> objectList)
        {

            try
            {
                dicos.Add(key, objectList);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [WebMethod]
        public List<object> getDataFromWebPart(string key)
        {
            try
            {
                return dicos[key];
            }
            catch (Exception)
            {

                return null;
            }
        }


        [WebMethod]
        public bool DeleteDataFromWebPart(string key)
        {
            try
            {
                dicos.Remove(key);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        [WebMethod]
        public bool setName(string XMLString,string type)
        {
            Object oObject;
            XmlSerializer oXmlSerializer = new XmlSerializer(Type.GetType(type));
            oObject = oXmlSerializer.Deserialize(new StringReader(XMLString));
            return true;
        }

    }
}
