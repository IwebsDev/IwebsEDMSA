using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.Web.UI;
using Galatee.Silverlight.Web.serviceweb;
using Galatee.Silverlight.Web.ServiceWeb;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Xml.Linq;


namespace Galatee.Silverlight.Web
{
   static public class Utility
    {
        public static string CurrentModule = string.Empty;
        public static string Rdlc = string.Empty;
       static public DataTable ConvertToDataTable<T>(List<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn.DefaultView.Table;
        }
       public static Previews.UcGeneric UcControl = null;
       static string RelativeLocation(Page p)
       {
         return Assembly.GetExecutingAssembly().GetName().Name;
       }

       /// <summary>
       ///  create a real smart datatable from request parameters' string 
       /// </summary>
       /// <typeparam name="T">Type of object to map with datatable row type ex Person will be set datatable row value </typeparam>
       /// <param name="param">request parameters' string </param>
       /// <param name="t">Type of object to pass as a report datasource type</param>
       /// <returns></returns>
       static public  DataTable getListeFromRequestParam<T>(string param,T t)
       {

           List<T> paramLISTE = new List<T>();
           DataTable dtReturn = new DataTable();

           foreach(string colum in getKey(param)) // constructin de colonne a partir des noms de cle
               dtReturn.Columns.Add(new DataColumn(colum, colum.GetType()));

           
           Dictionary<int,List<string>> dicos=getObjectPropValue(param);
           if(dicos != null && dicos.Count !=0)
           {
               foreach (KeyValuePair<int, List<string>> pair in dicos)
               {
                   int incr = -1;
                   DataRow dr = dtReturn.NewRow();
                   foreach (string values in pair.Value)
                   {
                       incr++;
                       List<string> column=getKey(param);
                       string columsNmae=column[incr];
                       dr[columsNmae] = values;
                       
                   }
                   dtReturn.Rows.Add(dr);
               }
                       
            }

           return dtReturn.DefaultView.Table;
       }

       /// <summary>
       /// Get all keys from Request qureryString ex: http://inova.ci/galatee.aspx?key1=values1&key2=values2
       /// </summary>
       /// <param name="param">string of request parameters</param>
       /// <returns></returns>
       static List<string> getKey(string param)
       {
           List<string> keyList = new List<string>(); // liste de toutes les cles de la paire cle/valeur contenue dans l'url
           List<string> DistinctkeyList = new List<string>(); // liste de toutes les cles de la paire cle/valeur contenue dans l'url
           List<string> splitParam = param.Split('&').ToList();
           foreach (string split in splitParam)
           {
               keyList.Add(split.Split('=')[0].Split('_')[0]);
           }
           DistinctkeyList.AddRange(keyList.Distinct());
           return DistinctkeyList;
       }

       /// <summary>
       /// Get all values from request querystring ex http://inova.ci/galatee.aspx?key1=values1&key2=values2
       /// </summary>
       /// <param name="param">string of request parameters</param>
       /// <returns></returns>
        static List<string> getValue(string param)
       {
           List<string> valueList = new List<string>(); // liste de toutes les cles de la paire cle/valeur contenue dans l'url
           List<string> DistinctkeyList = new List<string>(); // liste de toutes les cles de la paire cle/valeur contenue dans l'url
           List<string> splitParam = param.Split('&').ToList();
           foreach (string split in splitParam)
           {
               if(split.Split('=').Count() >= 2 && split.Split('=') !=null)
                   valueList.Add(split.Split('=')[1]);
               else
                   valueList.Add(string.Empty);
           }
           return valueList;
       }

       /// <summary>
       /// create a dictionnary from request parameters 
       /// ex : key:1  => List<string> relatiValue= new List<string>(){ string ,string } and that list will be set as row values in datatable
       /// </summary>
        /// <param name="par">string of request parameters</param>
       /// <returns></returns>
       static Dictionary<int,List<string>> getObjectPropValue(string par)
         {
             Dictionary<int, List<string>> innerdico = new Dictionary<int, List<string>>();
             try
             {
                 int beginEntryPar = 0;
                 int dicoKey = -1;
                 List<string> valueListe = new List<string>();
                 string flag = par.Split('&').ToList()[0].Split('=')[0].Split('_')[0];
                 foreach (string val in par.Split('&'))
                 {
                     string[] paramZone = val.Split('=');
                     beginEntryPar++;
                     if (beginEntryPar == 1)
                     {
                         valueListe.Add(paramZone[1]);
                     }
                     else
                     {
                         if (paramZone.Count() >= 2 && paramZone != null)
                         {
                             if (paramZone[0].Split('_')[0] != flag)
                                 valueListe.Add(paramZone[1]);
                             else // si le flag est atteint
                             {
                                 List<string> vals = new List<string>();
                                 vals.AddRange(valueListe);
                                 dicoKey++;
                                 innerdico.Add(dicoKey, vals);
                                 // beginEntryPar = 0;
                                 valueListe.Clear();

                                 valueListe.Add(paramZone[1]);
                             }
                         }
                     }
                 }
                 innerdico.Add(beginEntryPar, valueListe);
                 
             }
             catch (Exception ec)
             {

                 throw;
             }

             return innerdico;
         }

        static string getKeyFromUrlParam(Page p)
       {
           return p.Request.QueryString["key"];
       }

       //public static List<object> returnWebServiceData(Page p)
       //{
       //    printingservice service = new printingservice();
       //    return service.getDataFromWebService(getKeyFromUrlParam(p));
       //}

       //public static string getNameFromWebserve()
       //{
       //    printingservice soap = new printingservice();
       //    return  soap.getName();
       //}

       //public static List<CsReglement> getReglementFromWebserve()
       //{
       //    printingservice soap = new printingservice();
       //    return soap.getReglement();
       //}

       //public static List<object> getReglementOFromWebserve()
       //{
       //    printingservice soap = new printingservice();
       //    return soap.getReglementO();
       //}

        public static UserControl CreateUserControl(string UcName)
        {
            try
            {
                Type ucType = Assembly.GetExecutingAssembly().GetType(UcName);
                object ucObject = new object();
                ucObject = Activator.CreateInstance(ucType);
                UserControl uc = (UserControl)ucObject;
                return uc;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetServiceAddress(string module)
        {
            try
            {
                    string caissePath = HttpContext.Current.Server.MapPath(@"~/Config/ClientConfig.xml");
                    //String caissePath = System.Web.HttpServerUtility.MapPath();
                    String contenuXml = String.Empty;

                    using (FileStream stream = new FileStream(caissePath, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            List<ConfigClass> listeconfigs = new List<ConfigClass>();
                            XmlSerializer serializer = new XmlSerializer(typeof(ConfigClass));
                            contenuXml = reader.ReadToEnd();

                            XElement rootElement = XElement.Parse(contenuXml);
                            IEnumerable<XElement> caisses = rootElement.Descendants("ConfigClass");

                            foreach (XElement el in caisses)
                                using (MemoryStream memStream = new MemoryStream(Encoding.Unicode.GetBytes(el.ToString())))
                                {
                                    listeconfigs.Add((ConfigClass)serializer.Deserialize(memStream));
                                }

                            ConfigClass config = listeconfigs.FirstOrDefault(print => print.module.Equals(module));
                            return config.address;
                        }
                    }
            }
            catch (Exception ex)
            {
                //PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(),EndPoint());
                //printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
                throw ex;
            }
        }

    }
    
}