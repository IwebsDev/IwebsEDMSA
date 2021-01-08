using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Galatee.ModuleLoader.serviceWeb;
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using System.IO;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace Galatee.ModuleLoader
{
   static public class Utility
    {
       static string tag = string.Empty;
         // From: http://www.c-sharpcorner.com/UploadFile/VIMAL.LAKHERA/LINQResultsetToDatatable06242008042629AM/LINQResultsetToDatatable.aspx
       static public string CreateUrlFromList<T>(List<T> varlist)
        {
            // column names 
            PropertyInfo[] oProps = null;
            string urlBuit = string.Empty;

            if (varlist == null) return string.Empty ;
            int incr = 0;

            foreach (T rec in varlist)
            {
                incr++;
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
                        string values = pi.GetValue(rec, null) == null ? "V" : (string)pi.GetValue(rec, null).ToString();
                        urlBuit += pi.Name +"_"+ incr + "=" + values + "&";
                    }
                }
                oProps = null;
              }
            urlBuit = urlBuit.Substring(0, urlBuit.Length - urlBuit.Substring(urlBuit.LastIndexOf("&")).Length);
            return urlBuit;
        }

       static public string createStreamForDetailMoratoire(List<CsDetailMoratoire> detailmoratoires)
       {
           try
           {
               var s = new XmlSerializer(typeof(List<CsDetailMoratoire>));
               StringBuilder sb = new StringBuilder();

               XmlWriterSettings writerSettings = new XmlWriterSettings();
               writerSettings.OmitXmlDeclaration = true;

               XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
               ns.Add("", "");

               XmlWriter wr = XmlWriter.Create(sb, writerSettings);
               s.Serialize(wr, detailmoratoires, ns);

               string key = string.Format("{0}", sb.ToString());

               return key;
           }
           catch (Exception)
           {
               return string.Empty;
           }

       }

       static public string createXmlStringFromList<T>(List<T> detailmoratoires)
       {
           try
           {
               var s = new XmlSerializer(typeof(List<T>));
               StringBuilder sb = new StringBuilder();

               XmlWriterSettings writerSettings = new XmlWriterSettings();
               writerSettings.OmitXmlDeclaration = true;

               XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
               ns.Add("Objects", "http://www.w3.org/2005/Atom");

               XmlWriter wr = XmlWriter.Create(sb, writerSettings);
               s.Serialize(wr, detailmoratoires, ns);
              
               string key = string.Format("{0}", sb.ToString());

               return key;
           }
           catch (Exception)
           {
               return string.Empty;
           }

       }

       static public EndpointAddress EndPoint(string moduleNmae)
       {
           try
           {
               string currentModule = moduleNmae;
               return new EndpointAddress("");
               //return new EndpointAddress(((App)Application.Current).ModuleServiceConfig[currentModule].address);
           }
           catch (Exception ex)
           {
               string error = ex.Message;
               return null;
           }
       }

       static public BasicHttpBinding Protocole()
       {
           try
           {
               return new BasicHttpBinding(BasicHttpSecurityMode.None);
           }
           catch (Exception ex)
           {
               string error = ex.Message;
               return null;
           }
       }
   }
    
}