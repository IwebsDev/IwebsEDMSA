using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.Globalization;
using System.Xml.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;
using Galatee.Silverlight.Library;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Galatee.Silverlight
{
    public partial class App : Application
    {
        // CHK - 01/03/2013 
        FrmMain frmMain = new FrmMain();
        public FrmMain FrmMain
        {
            get { return frmMain; }
            set { frmMain = value; }
        }
        public Shared.UcPreviewReport preview = new Shared.UcPreviewReport();
        WebClient objWebClient { get; set; } 
        // variable globale de l'application silverlight

        Dictionary<string, ConfigClass> moduleServiceConfig = new Dictionary<string, ConfigClass>();

        public Dictionary<string, ConfigClass> ModuleServiceConfig
        {
            get { return moduleServiceConfig; }
            set { moduleServiceConfig = value; }
        }

        public string ServiceUrl
        { get; set; }

        public string BdUrl
        { get; set; }

        public App()
        {
            try
            {

                this.Startup += this.Application_Startup;
                this.Exit += this.Application_Exit;
                this.UnhandledException += this.Application_UnhandledException;


                Thread.CurrentThread.CurrentCulture = new CultureInfo("fr");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");

                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //this.RootVisual = new Caisse.UcConnexion(new Page());
            // Récupération du paramètre plugin
            // On récupère l'url à laquelle se trouve le XAP Silverlight
            String uri = Application.Current.Host.Source.AbsoluteUri;

            try
            {
                // On retourne à la racine du dossier du projet Web
                uri = uri.Substring(0, uri.IndexOf("/ClientBin")) + "/Config" + "/ClientConfig.xml";

                try
                {
                    // On créé le Webclient
                    WebClient objWebClient = new WebClient();
                    // on gère l'évenement "J'ai fini de télécharger le fichier XML"
                    objWebClient.DownloadStringCompleted += (sen, args) =>
                    {
                        // Si le télécghargement c'est bien passé et n'as pas été annulé
                        if (!args.Cancelled && args.Error == null)
                        {
                            List<ConfigClass> li = new List<ConfigClass>();
                            li.AddRange(Library.Utility.CreateCollectionFromXml<ConfigClass>(args.Result, "ConfigClass"));
                            // On convertis la String en objet XML
                            XElement myXml = XElement.Parse(args.Result);
                            // On parcours tout les neuds User
                            foreach (XElement el in (from el in myXml.Elements("ConfigClass") select el).ToList())
                            {
                                ConfigClass c = new ConfigClass()
                                {

                                    module = el.Element("module").Value,
                                    security = el.Element("security").Value,
                                    address = el.Element("address").Value,
                                    binding = el.Element("binding").Value,
                                    bindingConfiguration = el.Element("bindingConfiguration").Value,
                                    contract = el.Element("contract").Value,
                                    maxBufferSize = el.Element("maxBufferSize").Value,
                                    maxReceivedMessageSize = el.Element("maxReceivedMessageSize").Value
                                };

                                moduleServiceConfig.Add(c.module, c);

                                //li.Add(CreateObjectFromXml<ConfigClass>(el));
                            }
                            this.RootVisual = new FrmMain();
                            //if (e.InitParams.Count != 0)
                            //{
                            //    string MachineClient = string.Empty;
                            //    string IpAddress = string.Empty;
                            //    if (e.InitParams.ContainsKey("HostName"))
                            //    {
                            //        string[] Unparametre = e.InitParams["HostName"].Split('.');
                            //        MachineClient = Unparametre[0].ToUpper();
                            //    }
                            //    if (e.InitParams.ContainsKey("HostIP"))
                            //        IpAddress = e.InitParams["HostIP"];


                            //    Silverlight.Classes.IsolatedStorage.writeUserInfo(MachineClient.Trim(), "MachineClient");
                            //    Silverlight.Classes.IsolatedStorage.writeUserInfo(IpAddress, "IpAddress");
                            //}
                        }
                    };
                    objWebClient.DownloadStringAsync(new Uri(uri, UriKind.Absolute));

                    //this.RootVisual = new FrmMain();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void Application_Exit(object sender, EventArgs e)
        {
            try
            {
                //var mBoxControl = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Devis.Languages.txtDevis, "Voulez vous quitter l'application ? ", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //mBoxControl.OnMessageBoxClosed += (_, result) =>
                //{
                //    if (mBoxControl.Result == MessageBoxResult.OK)
                //    {

                //    }
                //    else
                //    {
                //        return;
                //    }
                //};
                //mBoxControl.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                // If the app is running outside of the debugger then report the exception using
                // the browser's exception mechanism. On IE this will display it a yellow alert 
                // icon in the status bar and Firefox will display a script error.
                if (!System.Diagnostics.Debugger.IsAttached)
                {

                    // NOTE: This will allow the application to continue running after an exception has been thrown
                    // but not handled. 
                    // For production applications this error handling should be replaced with something that will 
                    // report the error to the website and stop the application.
                    e.Handled = true;
                    Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                //string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.InnerException;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        T CreateObjectFromXml<T>(XElement monItem) where T : new()
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        object CreateOurNewObject(XElement monItem)
        {
            try
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
                return generetedObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class ConfigClass
    {
        public string module { get; set; }
        public string address { get; set; }
        public string binding { get; set; }
        public string bindingConfiguration { get; set; }
        public string maxBufferSize { get; set; }
        public string contract { get; set; }
        public string maxReceivedMessageSize { get; set; }
        public string security { get; set; }
        
    }
}
