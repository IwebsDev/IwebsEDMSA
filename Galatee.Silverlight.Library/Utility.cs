using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using System.Reflection.Emit;
using System.Threading;
using System.Xml.Linq;

namespace Galatee.Silverlight.Library
{
    public class Utility
    {

        static public string CreateUrlFromList<T>(List<T> varlist)
        {
            // column names 
            PropertyInfo[] oProps = null;
            string urlBuit = string.Empty;

            if (varlist == null) return string.Empty;
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
                        urlBuit += pi.Name + "_" + incr + "=" + values + "&";
                    }
                }
                oProps = null;
            }
            urlBuit = urlBuit.Substring(0, urlBuit.Length - urlBuit.Substring(urlBuit.LastIndexOf("&")).Length);
            return urlBuit;
        }

        static public string createXmlStringFromList<T>(List<T> ListeGeneric)
        {
            try
            {
                var s = new XmlSerializer(typeof(List<T>));
                StringBuilder sb = new StringBuilder();

                XmlWriterSettings writerSettings = new XmlWriterSettings();
                writerSettings.OmitXmlDeclaration = true;

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("Objects", "http://www.inova.ci/HGB/Atom");

                XmlWriter wr = XmlWriter.Create(sb, writerSettings);
                s.Serialize(wr, ListeGeneric, ns);

                string key = string.Format("{0}", sb.ToString());

                return key;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        static public T ParseObject<T, P>(T objetARemplir, P objetATransferer)
        {
            try
            {
                T objetARemp = objetARemplir;
                // Recuperation des types
                PropertyInfo[] properties1 = objetARemplir.GetType().GetProperties();
                PropertyInfo[] properties2 = objetATransferer.GetType().GetProperties();

                // Test de l'unicité des deux types
                if (properties1.Length == properties2.Length)
                {
                    // Remplacement des valeurs
                    for (int attrNum = 0; attrNum < properties2.Length; attrNum++)
                    {
                        if (properties1[attrNum].GetType().Equals(properties2[attrNum].GetType()))
                        {
                            object value2 = properties2[attrNum].GetValue(objetATransferer, null);
                            properties1[attrNum].SetValue(objetARemp, value2, null);
                        }
                    }

                    return objetARemp;

                }
                else
                    throw new Exception("Les types n'ont pas la meme structure");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        static public void Action(Dictionary<string, string> param, string printer, string rdlcUri, string moduleNmae)
        {
            // Parsing de originalArray vers Arrays
            string key = getKey();

            try
            {
                SendToPrinter(rdlcUri, moduleNmae, key, printer);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Cette méthode permet d'invoquer le fichier http handler en fonction du module 
        /// Elle passe ces différents paramètres comme QueryString à la requête http
        /// </summary>
        /// <param name="rdlcUri">Nom du fichier de rapport qui servira à l'impression</param>
        /// <param name="moduleNmae">Libellé du module dans lequel l'impression s'exécutera</param>
        /// <param name="key">Clé servant de sélection des données à éditer stockées dans le dictionnaire d'impression </param>
        /// <param name="printer">Nom de l'imprimante</param>
        static void SendToPrinter(string rdlcUri, string moduleNmae, string key, string printer)
        {
            string parametres = rdlcUri + "|" + moduleNmae + "|" + key + "|" + printer; ;

            WebClient objWebClient = new WebClient();
            objWebClient.UploadStringCompleted += (send, resul) =>
            {
                //SessionObject.CloseLoadingIndicator();
            };
            objWebClient.UploadStringAsync(GetHTTPHandlerURI(), parametres);
            //objWebClient.UploadStringAsync(GetHTTPHandlerURI(moduleNmae, rdlcUri), parametres);
        }

        /// <summary>
        /// Permet de determiner l'url du fichier handler http sur le serveur web
        /// NB ce fichier est le point d'entree de toute requete d'impression vers le projet silverlight web
        /// </summary>
        /// <param name="moduleName">module dans lequel se fera l'impression</param>
        /// <param name="url">chemin absolu du fichier http handler  servant de endpoint à toute requete d'impression du module passé en paramètre</param>
        /// <returns></returns>
        static public Uri GetHTTPHandlerURI(string moduleName, string url)
        {
            string strBaseWebAddress = Application.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = Application.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
            Uri UriWebService = new Uri(String.Format(@"{0}" + "ReportAspPage/" + moduleName + @"/" + url + ".ashx", root));
            return UriWebService;
        }

        /// <summary>
        /// Permet de determiner l'url du fichier http handler  sur le serveur web
        /// NB ce fichier est le point d'entree de toute requete d'impression vers le projet silverlight web
        /// NOM DU FICHIER HANDLER : GenericPrintHandler.ashx
        /// </summary>
        /// <author>HGB</author>
        /// <DATE>27/02/2013</DATE>
        /// <returns></returns>
        static public Uri GetHTTPHandlerURI()
        {
            //HandlerPrinting est un repertoire situé à la racine du projet web dans lequel 
            // se trouve le fichier http handler
            string handler = "GenericPrintHandler.ashx";
            string strBaseWebAddress = Application.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = Application.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);

            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler, root));

            return UriWebService;
        }

        /// <summary>
        /// Permet de determiner le chemin du Rapport .rdlc a imprimé
        /// à partir d'un projet silverlight
        /// </summary>
        /// <author>HGB</author>
        /// <DATE>27/02/2013</DATE>
        /// <param name="url">nom du fichier .rdlc a imprimé</param>
        /// <param name="moduleName">nom du module dans lequel ce trouve le rapport .rdlc</param>
        /// <param name="key">cle d'invocation de la page web</param>
        /// <returns></returns>
        static Uri GetBaseAddress(string rdlcName, string moduleName, string key)
        {
            try
            {
                //A la racine du projet web se trouve le repertoire ReportAspPage dans lequel chaque module à un sous repertoire
                // pour implementer sa logique d'impression
                string strBaseWebAddress = Application.Current.Host.Source.AbsoluteUri;
                int PositionOfClientBin = Application.Current.Host.Source.AbsoluteUri.ToLower().IndexOf(@"clientbin");
                string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                Uri UriWebService = new Uri(String.Format(@"{0}" + "ReportAspPage/" + moduleName +
                                             @"/" + rdlcName + ".aspx?key={1}&rdlc={2}&module={3}", root, key, rdlcName, moduleName));
                return UriWebService;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }

        }

        /// <summary>
        /// parametre qui sera pris en compte par le localReport declenché pour l'impression
        /// NB L'objet LocalReport se trouve exclusivement coté serveur web dans le projet silverlight web.
        /// </summary>
        /// /// <author>HGB</author>
        /// <DATE>27/02/2013</DATE>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string setParam(string param)
        {
            try
            {
                string strBaseWebAddress = Application.Current.Host.Source.AbsoluteUri;
                int PositionOfClientBin = Application.Current.Host.Source.AbsoluteUri.ToLower().IndexOf(@"clientbin");
                string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                string clientIpAddrss = root.Split(':')[1].Split('/')[2];
                return clientIpAddrss + "_" + param;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return string.Empty;
            }

        }

        /// <summary>
        /// Chaque client aura sa propre clé qu'il enverra au service pour la prise 
        /// enc compte de l'impression
        /// NB L'adresse IP fera office de clé pour chaque client
        /// </summary>
        /// <author>HGB</author>
        /// <DATE>27/02/2013</DATE>
        /// <returns></returns>
        public static string getKey()
        {
            try
            {
                string strBaseWebAddress = Application.Current.Host.Source.AbsoluteUri;
                int PositionOfClientBin =  Application.Current.Host.Source.AbsoluteUri.ToLower().IndexOf(@"clientbin");
                string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                string clientIpAddrss = root.Split(':')[1].Split('/')[2];
                return clientIpAddrss;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return string.Empty;
            }

        }

        /// <summary>
        /// Convertir une liste d'objets de type P en une liste 
        /// d'objet de type T
        /// NB Les objets de type T et P doivent avoir le meme nombre de propriétés de meme type 
        /// </summary>
        /// <author>HGB</author>
        /// <DATE>27/02/2013</DATE>
        /// <typeparam name="T">Type de retour</typeparam>
        /// <typeparam name="P">Type de départ</typeparam>
        /// <param name="ListeDobjects">Liste source de type P</param>
        /// <returns></returns>
        public static List<T> ConvertListType<T, P>(List<P> ListeDobjects) where T : new()
        {
            List<T> ListeDestination = new List<T>();
            T objVide = new T();
            foreach (P item in ListeDobjects)
            {
                objVide = Utility.ParseObject<T, P>(objVide, item);
                ListeDestination.Add(objVide);
                objVide = new T();
            }
            return ListeDestination;
        }


        static T CreateObjectFromXml<T>(XElement monItem) where T : new()
        {
            /// >>> MODIFICATION END

            // create a dynamic assembly and module
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = "tmpAssembly";
            AssemblyBuilder assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder module = assemblyBuilder.DefineDynamicModule("tmpModule");

            // create a new type builder
            TypeBuilder typeBuilder = module.DefineType("BindableRowCellCollection", TypeAttributes.Public | TypeAttributes.Class);

            // Loop over the attributes that will be used as the properties names in out new type
            /// >>> MODIFICATION START
            foreach (XElement node in monItem.Elements())
            {
                string propertyName = node.Name.ToString();
                /// >>> MODIFICATION END

                // Generate a private field
                FieldBuilder field = typeBuilder.DefineField("_" + propertyName, typeof(string), FieldAttributes.Private);
                // Generate a public property
                PropertyBuilder property =
                    typeBuilder.DefineProperty(propertyName,
                                     PropertyAttributes.None,
                                     typeof(string),
                                     new Type[] { typeof(string) });

                // The property set and property get methods require a special set of attributes:

                MethodAttributes GetSetAttr =
                    MethodAttributes.Public |
                    MethodAttributes.HideBySig;

                // Define the "get" accessor method for current private field.
                MethodBuilder currGetPropMthdBldr =
                    typeBuilder.DefineMethod("get_value",
                                               GetSetAttr,
                                               typeof(string),
                                               Type.EmptyTypes);

                // Intermediate Language stuff...
                ILGenerator currGetIL = currGetPropMthdBldr.GetILGenerator();
                currGetIL.Emit(OpCodes.Ldarg_0);
                currGetIL.Emit(OpCodes.Ldfld, field);
                currGetIL.Emit(OpCodes.Ret);

                // Define the "set" accessor method for current private field.
                MethodBuilder currSetPropMthdBldr =
                    typeBuilder.DefineMethod("set_value",
                                               GetSetAttr,
                                               null,
                                               new Type[] { typeof(string) });

                // Again some Intermediate Language stuff...
                ILGenerator currSetIL = currSetPropMthdBldr.GetILGenerator();
                currSetIL.Emit(OpCodes.Ldarg_0);
                currSetIL.Emit(OpCodes.Ldarg_1);
                currSetIL.Emit(OpCodes.Stfld, field);
                currSetIL.Emit(OpCodes.Ret);

                // Last, we must map the two methods created above to our PropertyBuilder to
                // their corresponding behaviors, "get" and "set" respectively.
                property.SetGetMethod(currGetPropMthdBldr);
                property.SetSetMethod(currSetPropMthdBldr);
            }

            // Generate our type
            Type generetedType = typeBuilder.CreateType();

            // Now we have our type. Let's create an instance from it:
            object generetedObject = Activator.CreateInstance(generetedType);

            // Loop over all the generated properties, and assign the values from our XML:
            PropertyInfo[] properties = generetedType.GetProperties();

            int propertiesCounter = 0;

            // Loop over the values that we will assign to the properties
            /// >>> MODIFICATION START
            foreach (XElement node in monItem.Elements())
            {
                string value = node.Value.ToString();
                /// >>> MODIFICATION END
                properties[propertiesCounter].SetValue(generetedObject, value, null);
                propertiesCounter++;
            }

            //Yoopy ! Return our new genereted object.
            T objRetour = new T();
            objRetour = Utility.ParseObject<T, object>(objRetour, generetedObject);
            return objRetour;
            //return generetedObject;
        }

        public static List<T> CreateCollectionFromXml<T>(string xmlString,string Racine) where T : new()
        {
            /// parser le flux xml 
            XElement myXml = XElement.Parse(xmlString);

            List<T> result = new List<T>();
            // On parcours tout les neuds du string xml 
            IEnumerable<XElement> EnumXmlElement = myXml.Elements(Racine);
            foreach (XElement el in EnumXmlElement)
                result.Add(CreateObjectFromXml<T>(el));

            return result;
        }


        //private void HandleLostFocus<T>(Library.NumericTextBox Code, TextBox Libelle, List<T> listItems)
        //{
        //    if (!string.IsNullOrEmpty(Code.Text) &&
        //        Code.Text.Length ==SessionObject.Enumere.TailleCodeQuartier &&
        //        listItems.Count != 0)
        //    {
        //        Code.Text = Code.Text.PadLeft(SessionObject.Enumere.TailleCodeQuartier, '0');
        //    }
        //    else
        //    {
        //        Code.Text = string.Empty;
        //        Libelle.Text = string.Empty;
        //    }
        //}
        //private void HandleLostFocus<T>(TextBox Code, TextBox Libelle, List<T> listItems)
        //{
        //    if (!string.IsNullOrEmpty(Code.Text) &&
        //        Code.Text.Length == SessionObject.Enumere.TailleCodeQuartier &&
        //        listItems.Count != 0)
        //    {
        //        Code.Text = Code.Text.PadLeft(SessionObject.Enumere.TailleCodeQuartier, '0');
        //    }
        //    else
        //    {
        //        Code.Text = string.Empty;
        //        Libelle.Text = string.Empty;
        //    }
        //}
    }
}
