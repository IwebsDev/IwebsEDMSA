using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Services;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Web.Hosting;
using System.Data;
using System.Text;
using System.Runtime.Serialization;
using System.Web.UI;

namespace Galatee.Silverlight.Web
{
    /// <summary>
    /// Description résumée de ModifyClientConfig
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")] 
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ModifyClientConfig : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
             int bytelength = context.Request.ContentLength;
            byte[] inputbytes = context.Request.BinaryRead(bytelength);
            //string message = System.Text.Encoding.Default.GetString(inputbytes);
            string message = System.Text.Encoding.UTF8.GetString(inputbytes);
            ConfigClass oObject =new ConfigClass();

            string filename ="ClientConfig.xml";
            List<ConfigClass> configs = DeSerializeAnObject(message);

            string path = string.Empty;
            
            try
            {
                path = @HostingEnvironment.ApplicationHost.GetPhysicalPath() + "Config\\" + filename;

                 //create the serialiser to create the xml
                    XmlSerializer serialiser = new XmlSerializer(typeof(List<ConfigClass>));
                    TextWriter Filestream =null;

                    if (File.Exists(path))
                    {
                        Filestream = new StreamWriter(path);

                        //write to the file
                        serialiser.Serialize(Filestream, configs);

                        // Close the file
                        Filestream.Close();
                    }
            }
            catch (Exception ex )
            {
                
                string error = ex.Message;
            }

            //DataSet oDS = new DataSet();

            //ConfigClass aTa0 = null;
            //DataRow row = null;
            ////- Après copie des fichiers...
            //oDS.ReadXml(path);
            //for (int i = 0; i < oDS.Tables[0].Rows.Count; i++)
            //{
            //    row = oDS.Tables[0].Rows[i];
            //    aTa0 = new ConfigClass()
            //    {
            //        Nom = row["Nom"].ToString(),
            //        Prenom = row["Prenom"].ToString(),
            //        Sexe = row["Sexe"].ToString()
            //    };
            //}
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("You have submitted data:" + message);
            context.Response.Write("true");
        }

        private List<ConfigClass> DeSerializeAnObject(string xmlOfAnObject)
        {
            List<ConfigClass> myObject = new List<ConfigClass>();

            System.IO.StringReader read = new StringReader(xmlOfAnObject);

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(myObject.GetType());

            System.Xml.XmlReader reader = new XmlTextReader(read);

            try
            {
                //using (var stream = new MemoryStream(Encoding.Unicode.GetBytes(xmlOfAnObject)))
                //{
                //    var serializer = new DataContractSerializer(typeof(ConfigClass));
                //    ConfigClass theObject = (ConfigClass)serializer.ReadObject(stream);
                //    return theObject;
                //}

                myObject = (List<ConfigClass>)serializer.Deserialize(reader);

                return myObject;

            }

            catch
            {

                throw;

            }

            finally
            {

                reader.Close();

                read.Close();

                read.Dispose();

            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}