using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using System.IO;
using System.ServiceModel;
using System.Windows.Controls;
using System.Windows;
using System.ServiceModel.Channels;
using Galatee.Silverlight.ServicePrintings;
using System.Net;
using System.Windows.Browser;
using Galatee.Silverlight.Resources.Accueil;

namespace Galatee.Silverlight
{
    public class Utility
    {
        static string tag = string.Empty;

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

        static public string createStreamForDetailMoratoire(List<ServicePrintings.CsDetailMoratoire> detailmoratoires)
        {
            try
            {
                var s = new XmlSerializer(typeof(List<ServicePrintings.CsDetailMoratoire>));
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

        static public EndpointAddress EndPoint(ChildWindow w)
        {
            try
            {
                // quant une erreur du service survient, le Tag du controle en cours devient null ?
                // donc le ramener au module en cours 
                string currentModule = w.Tag == null ? SessionObject.ModuleEnCours : w.Tag.ToString();
                string serviceUri = ((App)Application.Current).ModuleServiceConfig[currentModule].address;
                return new EndpointAddress(serviceUri);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        static public EndpointAddress EndPoint(string moduleNmae)
        {
            try
            {
                string currentModule = moduleNmae;
                return new EndpointAddress(((App)Application.Current).ModuleServiceConfig[currentModule].address);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        static public EndpointAddress EndPointCaisse()
        {
            try
            {
                string currentModule = "Caisse";
                string port = ((App)Application.Current).ModuleServiceConfig[currentModule].address.Split(':')[2].Split('/')[0].ToString();
                SessionObject.MachinePort = "2013";
                //SessionObject.MachinePort = port;
                string address = "http://localhost:" + SessionObject.MachinePort + "/AuthentInitialize/AuthentInitializeService.svc";
                //string address = "http://localhost:" + SessionObject.MachinePort + "/Caisse/CaisseService.svc";
                return new EndpointAddress(address);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        static public EndpointAddress EndPointCaisseOffLine()
        {
            try
            {
                string currentModule = "Caisse";
                string port = ((App)Application.Current).ModuleServiceConfig[currentModule].address.Split(':')[2].Split('/')[0].ToString();
                SessionObject.MachinePort = "2013";
                //SessionObject.MachinePort = port;
                string address = "http://localhost:" + SessionObject.MachinePort + "/AuthentInitialize/AuthentInitializeService.svc";
                return new EndpointAddress(address);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
        //static public EndpointAddress EndPoint(this  ChildWindow pThis)
        //{
        //    try
        //    {
        //        string _assembly = pThis.GetType().Namespace;
        //        string currentModule = (string)pThis.Tag ;
        //        return new EndpointAddress(((App)Application.Current).ModuleServiceConfig[currentModule].address);
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //        return null;
        //    }
        //}

        static public EndpointAddress EndPoint<T>(ClientBase<T> client, string module) where T : class
        {
            try
            {
                string currentModule = module;
                EndpointAddress endpoint = new EndpointAddress(((App)Application.Current).ModuleServiceConfig[currentModule].address);

                return endpoint;

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        //static public BasicHttpBinding Protocole(this ChildWindow pThis)
        //{
        //    try
        //    {
        //        BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
        //        //CustomBinding bindingCustom = new CustomBinding(binding);
        //        //HttpTransportBindingElement httpBindingElement = new HttpTransportBindingElement();

        //        //httpBindingElement.MaxBufferSize = Int32.MaxValue;
        //        //httpBindingElement.MaxReceivedMessageSize = Int32.MaxValue;

        //        //bindingCustom.Elements.Add(httpBindingElement);

        //        binding.MaxReceivedMessageSize = Int32.MaxValue;
        //        binding.MaxBufferSize = Int32.MaxValue;
        //        return binding;
        //        //return bindingCustom;
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //        return null;
        //    }
        //}


        static public BasicHttpBinding Protocole()
        {
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                binding.MaxReceivedMessageSize = Int32.MaxValue;
                binding.MaxBufferSize = Int32.MaxValue;
                return binding;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// Crée un Binding pour la liaison WCF du service facturation
        /// </summary>
        /// <returns>Un objet de liaison</returns>
        static public BasicHttpBinding ProtocoleFacturation()
        {
            try
            {
               
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                binding.OpenTimeout = new TimeSpan(5, 0, 0);
                binding.CloseTimeout = new TimeSpan(5, 0, 0);
                binding.SendTimeout = new TimeSpan(5, 0, 0);
                binding.ReceiveTimeout = new TimeSpan(5, 0, 0);

                binding.MaxReceivedMessageSize = Int32.MaxValue;
                binding.MaxBufferSize = Int32.MaxValue;
                binding.MaxBufferSize = Int32.MaxValue;
                return binding;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static public BasicHttpBinding ProtocoleIndex()
        {
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                binding.OpenTimeout = new TimeSpan(0, 1, 0);
                binding.CloseTimeout = new TimeSpan(5, 0, 0);
                binding.SendTimeout = new TimeSpan(5, 0, 0);

                binding.MaxReceivedMessageSize = Int32.MaxValue;
                binding.MaxBufferSize = Int32.MaxValue;
                return binding;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static public T ParseObject<T, P>(T objetARemplir, P objetATransferer)
        {
            int indexval = 0;
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
                        indexval = indexval + 1;
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
                int t = indexval;
                throw ex;
            }

        }
        public static List<T> ConvertListTypeByMaping<T, P>(List<P> ListeDobjects) where T : new()
        {
            try
            {
                // Parsing de originalArray vers Arrays
                List<T> ListeAImprimer = new List<T>();
                //for (int objNum = 0; objNum < ListeDobjects.Count; objNum++)
                T objVide = new T();
                foreach (P item in ListeDobjects)
                {
                    if (item != null)
                    {
                        objVide = Utility.MapObject<T, P>(objVide, item);
                        ListeAImprimer.Add(objVide);
                        objVide = new T();
                    }
                }
                return ListeAImprimer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
        static public T MapObject<T, P>(T objetARemplir, P objetATransferer)
        {
            int indexval = 0;
            try
            {
                T objetARemp = objetARemplir;
                // Recuperation des types
                PropertyInfo[] properties1 = objetARemplir.GetType().GetProperties();
                PropertyInfo[] properties2 = objetATransferer.GetType().GetProperties();

                // Test de l'unicité des deux types
                //if (properties1.Length == properties2.Length)
                //{
                // Remplacement des valeurs

                for (int attrNum = 0; attrNum < properties2.Length; attrNum++)
                {
                    indexval = indexval + 1;


                    var Prop = properties1.FirstOrDefault(p => p.Name == properties2[attrNum].Name);
                    int indexofProp = properties1.ToList().IndexOf(Prop);
                    if (Prop != null)
                    {
                        object value2 = properties2[attrNum].GetValue(objetATransferer, null);
                        properties1[indexofProp].SetValue(objetARemp, value2, null);
                    }
                }

                return objetARemp;

                //}
                //else
                //    throw new Exception("Les types n'ont pas la meme structure");
            }
            catch (Exception ex)
            {
                int t = indexval;
                throw ex;
            }

        }

        static public bool HasSamePropertyValue<T>(T pObject1, T pObject2)
        {
            bool isSameValue = false;
            try
            {

                // Recuperation des types
                PropertyInfo[] properties1 = pObject1.GetType().GetProperties();
                PropertyInfo[] properties2 = pObject2.GetType().GetProperties();

                // Test de l'unicité des deux types
                if (properties1.Length == properties2.Length)
                {
                    // Remplacement des valeurs
                    for (int attrNum = 0; attrNum < properties2.Length; attrNum++)
                    {
                        if (properties1[attrNum].GetType().Equals(properties2[attrNum].GetType()))
                        {
                            object value2 = properties2[attrNum].GetValue(pObject2, null) == null ? string.Empty : properties2[attrNum].GetValue(pObject2, null);
                            object value1 = properties1[attrNum].GetValue(pObject1, null) == null ? string.Empty : properties1[attrNum].GetValue(pObject1, null);
                            if (value1.ToString() == value2.ToString())
                                isSameValue = true;
                            else
                            {
                                isSameValue = false;
                                break;
                            }
                        }
                    }

                    return isSameValue;

                }
                else
                    throw new Exception("Les types n'ont pas la meme structure");
            }
            catch (Exception ex)
            {
                return isSameValue;
            }

        }

        static public void Action<T, P>(List<T> ListeVide, List<P> ListeDobjects, Dictionary<string, string> param, string printer, string rdlcUri, string moduleNmae) where T : new()
        {
            // Parsing de originalArray vers Arrays
            try
            {
                List<T> ListeAImprimer = new List<T>();
                string key = getKey();

                T objVide = new T();
                foreach (P item in ListeDobjects)
                {

                    objVide = Utility.ParseObject<T, P>(objVide, item);
                    ListeAImprimer.Add(objVide);
                    objVide = new T();
                }

                List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
                foreach (T item in ListeAImprimer)
                    list.Add(item as Galatee.Silverlight.ServicePrintings.CsPrint);

                PrintingsServiceClient service = new PrintingsServiceClient();
                service.setFromWebPartCompleted += (sender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.Show("Erreur a l'exécution du service", "setDataFromWebPart");
                            return;
                        }

                        if (args.Result == true)
                        {
                            SendToPrinter(rdlcUri, moduleNmae, key, printer);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                };
                service.setFromWebPartAsync(list, key, param);
            }
            catch (Exception ex )
            {
                throw ex;
            }
        }

        static public void ActionExportation<T, P>(List<P> ListeDobjects, Dictionary<string, string> param, string NomFichier, string printer, string rdlcUri, string moduleNmae, bool IsLandscape, string Format) where T : new()
        {
            // Parsing de originalArray vers Arrays
            int loaderHandler = -1;
            loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
            try
            {
                List<T> ListeAImprimer = new List<T>();
                string key = getKey();
                T objVide = new T();
                foreach (P item in ListeDobjects)
                {
                    objVide = Utility.ParseObject<T, P>(objVide, item);
                    ListeAImprimer.Add(objVide);
                    objVide = new T();
                }

                List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
                foreach (T item in ListeAImprimer)
                    list.Add(item as Galatee.Silverlight.ServicePrintings.CsPrint);
                Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Printing"));
                PrintingsServiceClient service = new PrintingsServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Printing"));
                service.setFromWebPartAsync(list, key, param);
                service.setFromWebPartCompleted += (sender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            LoadingManager.EndLoading(loaderHandler);
                            Message.Show("Erreur a l'exécution du service", "Erreur EcritureService");
                            return;
                        }
                        if (args.Result == true)
                        {
                            SendToExport(NomFichier, rdlcUri, moduleNmae, key, printer, IsLandscape, Format);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoadingManager.EndLoading(loaderHandler);
                        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                    finally
                    {
                        LoadingManager.EndLoading(loaderHandler);
                    }

                };
            }
            catch (Exception ex)
            {

                LoadingManager.EndLoading(loaderHandler);
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        //static public void ActionExportation<T,>(List<P> ListeDobjects, Dictionary<string, string> param, string NomFichier, string printer, string rdlcUri, string moduleNmae, bool IsLandscape, string Format) where T : new()
        //{
        //    // Parsing de originalArray vers Arrays
        //    int loaderHandler = -1;
        //    loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
        //    try
        //    {
        //        List<T> ListeAImprimer = new List<T>();
        //        string key = getKey();
        //        T objVide = new T();
        //        foreach (P item in ListeDobjects)
        //        {
        //            objVide = Utility.ParseObject<T, P>(objVide, item);
        //            ListeAImprimer.Add(objVide);
        //            objVide = new T();
        //        }

        //        List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
        //        foreach (T item in ListeAImprimer)
        //            list.Add(item as Galatee.Silverlight.ServicePrintings.CsPrint);
        //        Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Printing"));
        //        PrintingsServiceClient service = new PrintingsServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Printing"));
        //        service.setFromWebPartAsync(list, key, param);
        //        service.setFromWebPartCompleted += (sender, args) =>
        //        {
        //            try
        //            {
        //                if (args.Cancelled || args.Error != null)
        //                {
        //                    string error = args.Error.Message;
        //                    LoadingManager.EndLoading(loaderHandler);
        //                    Message.Show("Erreur a l'exécution du service", "Erreur EcritureService");
        //                    return;
        //                }
        //                if (args.Result == true)
        //                {
        //                    SendToExport(NomFichier, rdlcUri, moduleNmae, key, printer, IsLandscape, Format);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                LoadingManager.EndLoading(loaderHandler);
        //                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
        //            }
        //            finally
        //            {
        //                LoadingManager.EndLoading(loaderHandler);
        //            }

        //        };
        //    }
        //    catch (Exception ex)
        //    {

        //        LoadingManager.EndLoading(loaderHandler);
        //        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
        //    }
        //}
        static public void ActionGenereFichierPlat<T, P>(List<P> ListeDobjects, string printer, string moduleNmae,string TypeFichier) where T : new()
        {
            // Parsing de originalArray vers Arrays
            int loaderHandler = -1;
            loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
            try
            {
                List<T> ListeAImprimer = new List<T>();
                string key = getKey();
                T objVide = new T();
                foreach (P item in ListeDobjects)
                {
                    objVide = Utility.ParseObject<T, P>(objVide, item);
                    ListeAImprimer.Add(objVide);
                    objVide = new T();
                }
                Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Printing"));

                List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
                foreach (T item in ListeAImprimer)
                    list.Add(item as Galatee.Silverlight.ServicePrintings.CsPrint);

                PrintingsServiceClient service = new PrintingsServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Printing"));
                service.setFromWebPartAsync(list, key, null );
                service.setFromWebPartCompleted += (sender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            LoadingManager.EndLoading(loaderHandler);
                            Message.Show("Erreur a l'exécution du service", "setDataFromWebPart");
                            return;
                        }

                        if (args.Result == true)
                        {
                            SendToFichierPlat(moduleNmae, key, printer, TypeFichier);
                        }
                    }
                    catch (Exception ex)
                    {
                        LoadingManager.EndLoading(loaderHandler);
                        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                    finally
                    {
                        LoadingManager.EndLoading(loaderHandler);
                    }

                };
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(loaderHandler);
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        static public void ActionDirectOrientation<T, P>(List<P> ListeDobjects, Dictionary<string, string> param, string printer, string rdlcUri, string moduleNmae, bool IsLandscape) where T : new()
        {
            // Parsing de originalArray vers Arrays
            int loaderHandler = -1;
            loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
            try
            {
                List<T> ListeAImprimer = new List<T>();
                string key = getKey();
                T objVide = new T();
                foreach (P item in ListeDobjects)
                {
                    objVide = Utility.ParseObject<T, P>(objVide, item);
                    ListeAImprimer.Add(objVide);
                    objVide = new T();
                }
                Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Printing"));

                List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
                foreach (T item in ListeAImprimer)
                    list.Add(item as Galatee.Silverlight.ServicePrintings.CsPrint);

                PrintingsServiceClient service = new PrintingsServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Printing"));
                //Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.setFromWebPartAsync(list, key, param);
                service.setFromWebPartCompleted += (sender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            LoadingManager.EndLoading(loaderHandler);
                            Message.Show("Erreur a l'exécution du service", "setDataFromWebPart");
                            return;
                        }

                        if (args.Result == true)
                        {
                            SendToPrinterOrientation(rdlcUri, moduleNmae, key, printer, IsLandscape);
                        }
                    }
                    catch (Exception ex) 
                    {
                        LoadingManager.EndLoading(loaderHandler);
                        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                    finally
                    {
                        LoadingManager.EndLoading(loaderHandler);
                    }

                };
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(loaderHandler);
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }


        static int compteur = 0;
        static public void ActionExportFormatWithSplitingPrinting<T, P>(List<P> ListeDobjects, Dictionary<string, string> param, string printer, string rdlcUri, string moduleNmae, bool IsLandscape,string Format) where T : new()
        {
            // Parsing de originalArray vers Arrays
            int loaderHandler = -1;
            loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
            try
            {
                List<T> ListeAImprimer = new List<T>();
                string key = getKey();
                T objVide = new T();
                foreach (P item in ListeDobjects)
                {
                    objVide = Utility.ParseObject<T, P>(objVide, item);
                    ListeAImprimer.Add(objVide);
                    objVide = new T();
                }
                Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Printing"));

                List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
                foreach (T item in ListeAImprimer)
                    list.Add(item as Galatee.Silverlight.ServicePrintings.CsPrint);


                SetFromWebPartWithSplitGeneriquePrinting(list, param, "", printer, rdlcUri, moduleNmae, IsLandscape, Format, loaderHandler, key, 2000);

            }
            catch (Exception ex)
            {

                LoadingManager.EndLoading(loaderHandler);
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        private static void SetFromWebPartWithSplitGeneriquePrinting(List<ServicePrintings.CsPrint> ListeDobjects, Dictionary<string, string> param, string NomFichier, string printer, string rdlcUri, string moduleNmae, bool IsLandscape, string Format, int loaderHandler, string key, int Offset)
        {
            var Reste = ListeDobjects.Count() % Offset;
            var Quotient = ListeDobjects.Count() / Offset;
            //Message.Show("RecursifSet debut " , "");
            RecursifSetPrinting(ListeDobjects, param, NomFichier, printer, rdlcUri, moduleNmae, IsLandscape, Format, loaderHandler, key, Offset, Quotient, Reste);
        }
        private static void RecursifSetPrinting(List<ServicePrintings.CsPrint> ListeDobjects, Dictionary<string, string> param, string NomFichier, string printer, string rdlcUri, string moduleNmae, bool IsLandscape, string Format, int loaderHandler, string key, int Offset, int Quotient, int Reste)
        {
            ServicePrintings.PrintingsServiceClient service = new ServicePrintings.PrintingsServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Printing"));
            service.setFromWebPartWithSplitAsync(ListeDobjects.GetRange(compteur * Offset, (compteur == Quotient) ? Reste : Offset), key, param);
            service.setFromWebPartWithSplitCompleted += (sender, args) =>
            {
                try
                {
                    //Message.Show("Retour  setFromWebPartWithSplit " + compteur, "");

                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        LoadingManager.EndLoading(loaderHandler);
                        Message.Show("Erreur a l'exécution du service", "setDataFromWebPart");
                        return;
                    }

                    if (args.Result == "ok")
                    {
                        //Message.Show("Retour  setFromWebPartWithSplit " +compteur, "");

                        compteur++;
                        if (compteur < Quotient)
                            RecursifSetPrinting(ListeDobjects, param, NomFichier, printer, rdlcUri, moduleNmae, IsLandscape, Format, loaderHandler, key, Offset, Quotient, Reste);
                        else
                        {
                            if (compteur == Quotient)
                            {
                                //Offset = Reste;
                                RecursifSetPrinting(ListeDobjects, param, NomFichier, printer, rdlcUri, moduleNmae, IsLandscape, Format, loaderHandler, key, Offset, Quotient, Reste);
                            }
                        }
                        if (compteur > Quotient)
                        {
                            compteur = 0;
                            //Message.Show("SendToExport debut" , "");
                            SendToExport(NomFichier, rdlcUri, moduleNmae, key, printer, IsLandscape, Format);
                            LoadingManager.EndLoading(loaderHandler);

                        }
                    }
                }
                catch (Exception ex)
                {
                    LoadingManager.EndLoading(loaderHandler);
                    Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                }
                finally
                {
                    //LoadingManager.EndLoading(loaderHandler);
                }

            };
        }
        static public void ActionCaissePrint(string key, string rdlcUri, string moduleNmae)
        {
            // Parsing de originalArray vers Arrays
            try
            {
                SendToCaissePrinter(rdlcUri, moduleNmae, key);
            }
            catch (Exception ex)
            {
                //Message.Show(ex.Message, "ActionCaisse=>" + Galatee.Silverlight.Resources.Langue.errorTitle);
                throw ex;
            }
        }
        static public void ActionCaisse<T, P>(List<P> ListeDobjects,string key , Dictionary<string, string> param,string rdlcUri, string moduleNmae) where T : new()
        {
            // Parsing de originalArray vers Arrays
            try
            {
                List<T> ListeAImprimer = new List<T>();
                int loaderHandler = -1;
                //string key = getKey();
                loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
                T objVide = new T();
                foreach (P item in ListeDobjects)
                {
                    objVide = Utility.ParseObject<T, P>(objVide, item);
                    ListeAImprimer.Add(objVide);
                    objVide = new T();
                }

                List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
                foreach (T item in ListeAImprimer)
                    list.Add(item as Galatee.Silverlight.ServicePrintings.CsPrint);

                PrintingsServiceClient service = new PrintingsServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Printing"));
                service.setFromWebPartCompleted += (sender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            LoadingManager.EndLoading(loaderHandler);
                            Message.Show("Erreur a l'exécution du service", "setDataFromWebPart");
                            return;
                        }

                        if (args.Result == true)
                            SendToCaissePrinter(rdlcUri, moduleNmae,key);
                    }
                    catch (Exception ex) { Message.Show(ex.Message,Galatee.Silverlight.Resources.Langue.errorTitle); }
                    finally
                    {
                        LoadingManager.EndLoading(loaderHandler);
                    }

                };
                service.setFromWebPartAsync(list, key, param);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        static public void Action<T, P>(List<P> ListeDobjects, Dictionary<string, string> param, string printer, string rdlcUri, string moduleNmae) where T : new()
        {
            // Parsing de originalArray vers Arrays
            try
            {
                List<T> ListeAImprimer = new List<T>();
                int loaderHandler = -1;
                string key = getKey();
                loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
                T objVide = new T();
                foreach (P item in ListeDobjects)
                {
                    objVide = Utility.ParseObject<T, P>(objVide, item);
                    ListeAImprimer.Add(objVide);
                    objVide = new T();
                }

                List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
                foreach (T item in ListeAImprimer)
                    list.Add(item as Galatee.Silverlight.ServicePrintings.CsPrint);

                PrintingsServiceClient service = new PrintingsServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Printing"));
                service.setFromWebPartCompleted += (sender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            LoadingManager.EndLoading(loaderHandler);
                            Message.Show("Erreur a l'exécution du service", "setDataFromWebPart");
                            return;
                        }

                        if (args.Result == true)
                        {

                            SendToPrinter(rdlcUri, moduleNmae, key, printer);
                        }
                    }
                    catch (Exception ex) { Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle); }
                    finally
                    {
                        LoadingManager.EndLoading(loaderHandler);
                    }

                };
                service.setFromWebPartAsync(list, key, param);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        static public void Action<T, P>(List<P> ListeDobjects,string key, Dictionary<string, string> param, string printer, string rdlcUri, string moduleNmae) where T : new()
        {
            // Parsing de originalArray vers Arrays
            try
            {
                List<T> ListeAImprimer = new List<T>();
                int loaderHandler = -1;
                //string key = getKey();
                loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
                T objVide = new T();
                foreach (P item in ListeDobjects)
                {
                    objVide = Utility.ParseObject<T, P>(objVide, item);
                    ListeAImprimer.Add(objVide);
                    objVide = new T();
                }

                List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
                foreach (T item in ListeAImprimer)
                    list.Add(item as Galatee.Silverlight.ServicePrintings.CsPrint);

                PrintingsServiceClient service = new PrintingsServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Printing"));
                service.setFromWebPartCompleted += (sender, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            LoadingManager.EndLoading(loaderHandler);
                            Message.Show("Erreur a l'exécution du service", "setDataFromWebPart");
                            return;
                        }

                        if (args.Result == true)
                        {

                            SendToPrinter(rdlcUri, moduleNmae, key, printer);
                        }
                    }
                    catch (Exception ex) { Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle); }
                    finally
                    {
                        LoadingManager.EndLoading(loaderHandler);
                    }

                };
                service.setFromWebPartAsync(list, key, param);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        static public void ActionMail<T, P>(List<P> ListeDobjects, Dictionary<string, string> param, string rdlcUri, string moduleNmae) where T : new()
        {
            // Parsing de originalArray vers Arrays
            List<T> ListeAImprimer = new List<T>();
            int loaderHandler = -1;
            string key = getKey();
            loaderHandler = LoadingManager.BeginLoading("Envoi en cours... ");
            T objVide = new T();
            foreach (P item in ListeDobjects)
            {
                objVide = Utility.ParseObject<T, P>(objVide, item);
                ListeAImprimer.Add(objVide);
                objVide = new T();
            }

            List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
            foreach (T item in ListeAImprimer)
                list.Add(item as Galatee.Silverlight.ServicePrintings.CsPrint);

            PrintingsServiceClient service = new PrintingsServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Printing"));
            service.setFromWebPartCompleted += (sender, args) =>
            {
                try
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        LoadingManager.EndLoading(loaderHandler);
                        Message.Show("Erreur a l'exécution du service", "setDataFromWebPart");
                        return;
                    }

                    if (args.Result == true)
                    {

                        SendToMailHandler(rdlcUri, moduleNmae, key);
                        LoadingManager.EndLoading(loaderHandler);

                    }
                }
                catch (Exception) { }
                finally
                {
                    LoadingManager.EndLoading(loaderHandler);
                }

            };
            service.setFromWebPartAsync(list, key, param);
        }


        static public void ActionPreviewAccueil<T, P>(List<P> ListeDobjects, Dictionary<string, string> param, string rdlcUri, string moduleNmae) where T : new()
        {
            // Parsing de originalArray vers Arrays
            List<T> ListeAImprimer = new List<T>();
            int loaderHandler = -1;
            string key = getKey();
            loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
            T objVide = new T();
            foreach (P item in ListeDobjects)
            {
                objVide = Utility.ParseObject<T, P>(objVide, item);
                ListeAImprimer.Add(objVide);
                objVide = new T();
            }

            List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
            foreach (T item in ListeAImprimer)
                list.Add(item as Galatee.Silverlight.ServicePrintings.CsPrint);

            PrintingsServiceClient service = new PrintingsServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Printing"));
            service.setFromWebPartCompleted += (sender, args) =>
            {
                try
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        LoadingManager.EndLoading(loaderHandler);
                        Message.Show("Erreur a l'exécution du service", "setDataFromWebPart");
                        return;
                    }

                    if (args.Result == true)
                    {

                        SendToHandlerCaisse(rdlcUri, moduleNmae, key);
                    }
                }
                catch (Exception) { }
                finally
                {
                    LoadingManager.EndLoading(loaderHandler);
                }

            };
            service.setFromWebPartAsync(list, key, param);
        }

        static public void ActionPreviewCaisse<T, P>(List<P> ListeDobjects, Dictionary<string, string> param, string rdlcUri, string moduleNmae) where T : new()
        {
            // Parsing de originalArray vers Arrays
            List<T> ListeAImprimer = new List<T>();
            int loaderHandler = -1;
            string key = getKey();
            loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
            T objVide = new T();
            foreach (P item in ListeDobjects)
            {
                objVide = Utility.ParseObject<T, P>(objVide, item);
                ListeAImprimer.Add(objVide);
                objVide = new T();
            }

            List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
            foreach (T item in ListeAImprimer)
                list.Add(item as Galatee.Silverlight.ServicePrintings.CsPrint);

            PrintingsServiceClient service = new PrintingsServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Printing"));
            service.setFromWebPartCompleted += (sender, args) =>
            {
                try
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        LoadingManager.EndLoading(loaderHandler);
                        Message.Show("Erreur a l'exécution du service", "setDataFromWebPart");
                        return;
                    }

                    if (args.Result == true)
                    {

                        SendToHandlerCaisse(rdlcUri, moduleNmae, key);
                    }
                }
                catch (Exception) { }
                finally
                {
                    LoadingManager.EndLoading(loaderHandler);
                }

            };
            service.setFromWebPartAsync(list, key, param);
        }
        static public void ActionPreview<T, P>(List<P> ListeDobjects, Dictionary<string, string> param, string rdlcUri, string moduleNmae) where T : new()
        {
            // Parsing de originalArray vers Arrays
            List<T> ListeAImprimer = new List<T>();
            int loaderHandler = -1;
            string key = getKey();
            loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
            T objVide = new T();
            foreach (P item in ListeDobjects)
            {
                objVide = Utility.ParseObject<T, P>(objVide, item);
                ListeAImprimer.Add(objVide);
                objVide = new T();
            }

            List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
            foreach (T item in ListeAImprimer)
                list.Add(item as Galatee.Silverlight.ServicePrintings.CsPrint);

            Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Printing"));

            PrintingsServiceClient service = new PrintingsServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Printing"));
            service.setFromWebPartCompleted += (sender, args) =>
            {
                try
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        LoadingManager.EndLoading(loaderHandler);
                        Message.Show("Erreur a l'exécution du service", "setDataFromWebPart");
                        return;
                    }

                    if (args.Result == true)
                    {
                        SendToHandlerAccueil(rdlcUri, moduleNmae, key);
                        LoadingManager.EndLoading(loaderHandler);
                    }
                }
                catch (Exception) {
                    LoadingManager.EndLoading(loaderHandler);
                }
                finally
                {
                }

            };
            service.setFromWebPartAsync(list, key, param);
        }
        static public void ActionPreviews<T, P>(List<P> ListeDobjects, Dictionary<string, string> param, string rdlcUri, string moduleNmae) where T : new()
        {
            // Parsing de originalArray vers Arrays
            List<T> ListeAImprimer = new List<T>();
            int loaderHandler = -1;
            string key = getKey();
            loaderHandler = LoadingManager.BeginLoading("Patientez  ... ");
            T objVide = new T();
            foreach (P item in ListeDobjects)
            {
                objVide = Utility.ParseObject<T, P>(objVide, item);
                ListeAImprimer.Add(objVide);
                objVide = new T();
            }

            List<Galatee.Silverlight.ServicePrintings.CsPrint> list = new List<ServicePrintings.CsPrint>();
            foreach (T item in ListeAImprimer)
            {
                object obj = item;
                list.Add((CsPrint)obj);
            }

            PrintingsServiceClient service = new PrintingsServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Printing"));
            service.setFromWebPartCompleted += (sender, args) =>
            {
                try
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        LoadingManager.EndLoading(loaderHandler);
                        Message.Show("Erreur a l'exécution du service", "setDataFromWebPart");
                        return;
                    }

                    if (args.Result == true)
                    {

                        SendToPreviewHandler(rdlcUri, moduleNmae, key);
                    }
                }
                catch (Exception) { }
                finally
                {
                    LoadingManager.EndLoading(loaderHandler);
                }

            };
            service.setFromWebPartAsync(list, key, param);
        }


        static public void ActionPreview<T, P>(Dictionary<string, string> param, string rdlcUri, string moduleNmae,string key) where T : new()
        {
            // Parsing de originalArray vers Arrays
            List<T> ListeAImprimer = new List<T>();
            int loaderHandler = -1;
            loaderHandler = LoadingManager.BeginLoading("Operation en cours ... ");
                try
                {
                  SendToPreviewHandler(rdlcUri, moduleNmae, key);

                }
                catch (Exception) { }
                finally
                {
                    LoadingManager.EndLoading(loaderHandler);
                }

          
        }
        static public void ActionPreviewDevis<T, P>(Dictionary<string, string> param, string rdlcUri, string moduleNmae, string key) where T : new()
        {
            // Parsing de originalArray vers Arrays
            List<T> ListeAImprimer = new List<T>();
            int loaderHandler = -1;
            loaderHandler = LoadingManager.BeginLoading("Operation en cours ... ");
            try
            {
                SendToPreviewHandlerDevis(rdlcUri, moduleNmae, key);

            }
            catch (Exception) { }
            finally
            {
                LoadingManager.EndLoading(loaderHandler);
            }


        }
        static public void ActionPreview<T>(Dictionary<string, string> param, string rdlcUri, string moduleNmae, string key) where T : new()
        {
            // Parsing de originalArray vers Arrays
            List<T> ListeAImprimer = new List<T>();
            int loaderHandler = -1;
            loaderHandler = LoadingManager.BeginLoading("Patientez s'il vous plait ... ");
            try
            {
                SendToPreviewHandler(rdlcUri, moduleNmae, key);

            }
            catch (Exception) { }
            finally
            {
                LoadingManager.EndLoading(loaderHandler);
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
            catch (Exception ex) { throw ex; }
        }

        static public void ActionImpressionDirect(string printer,string key, string rdlcUri, string moduleNmae) 
        {
            // Parsing de originalArray vers Arrays
            //string key = getKey();
            try
            {
                SendToPrinter(rdlcUri, moduleNmae, key, printer);
            }
            catch (Exception ex) { throw ex; }
        }
        static public void ActionImpressionDirectOrientation(string printer, string key, string rdlcUri, string moduleNmae,bool IsPaysage)
        {
            // Parsing de originalArray vers Arrays
            //string key = getKey();
            try
            {
                SendToPrinterOrientation(rdlcUri, moduleNmae, key, printer, IsPaysage);
            }
            catch (Exception ex) { throw ex; }
        }
        static public void ActionCreationDirect(string printer, string key, string rdlcUri, string moduleNmae)
        {
            // Parsing de originalArray vers Arrays
            //string key = getKey();
            try
            {
                SendToFile(rdlcUri, moduleNmae, key, printer);
            }
            catch (Exception ex) { throw ex; }
        }
        static public void Action(string printer, string rdlcUri, string moduleNmae)
        {
            // Parsing de originalArray vers Arrays
            string key = getKey();
            try
            {
                SendToPrinter(rdlcUri, moduleNmae, key, printer);
            }
            catch (Exception) { }
        }

        static public void ActionCaisseBanking(string key, string rdlcUri, string moduleNmae, bool IsOffline)
        {
            // Parsing de originalArray vers Arrays
            try
            {
                SendToCaissePrinter(rdlcUri, moduleNmae, key, IsOffline);
            }
            catch (Exception ex)
            {
                //Message.Show(ex.Message, "ActionCaisse=>" + Galatee.Silverlight.Resources.Langue.errorTitle);
                throw ex;
            }
        }

        static public void ActionPreview(string key,string printer, string rdlcUri, string moduleNmae)
        {
            try
            {
                SendToPreviewHandler(rdlcUri, moduleNmae, key);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Cette méthode permet d'invoquer le fichier http handler en fonction du module 
        /// Elle passe ces différents paramètres comme QueryString à la requête http
        /// </summary>
        /// <param name="rdlcUri">Nom du fichier de rapport qui servira à l'impression</param>
        /// <param name="moduleNmae">Libellé du module dans lequel l'impression s'exécutera</param>
        /// <param name="key">Clé servant de sélection des données à éditer stockées dans le dictionnaire d'impression </param>
        /// <param name="printer">Nom de l'imprimante</param>
        static void SendToCaissePrinter(string rdlcUri, string moduleNmae,string pKey)
        {
            string machineName = SessionObject.MachineName;
            string key = getKey();
            string printer = SessionObject.DefaultPrinter;
            string port = SessionObject.MachinePort;
            string parametres = rdlcUri + "|" + moduleNmae + "|" + pKey + "|" + printer + "|" + port;
            int loaderHandler = -1;
            loaderHandler = LoadingManager.BeginLoading("Impression en cours  ... ");
            //SendToWebPage(rdlcUri, moduleNmae, key, printer);
            WebClient objWebClient = new WebClient();
            objWebClient.UploadStringCompleted += (send, resul) =>
            {
                if (resul.Cancelled || resul.Error != null)
                {
                    string error = resul.Error.Message;
                    Message.Show(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    LoadingManager.EndLoading(loaderHandler);
                    return;
                }
                //Message.Show(Galatee.Silverlight.Resources.Langue.confirmPrintSuccess, Galatee.Silverlight.Resources.Langue.informationTitle);
                LoadingManager.EndLoading(loaderHandler);
            };
            objWebClient.UploadStringAsync(GetMainURI(rdlcUri, moduleNmae, pKey, printer, port, machineName), parametres);

        }

        static void SendToCaissePrinter(string rdlcUri, string moduleNmae, string pKey, bool IsOffline)
        {

            try
            {
                string machineName = SessionObject.MachineName;
                string portService = SessionObject.MachinePort;
                string printer = SessionObject.DefaultPrinter;

                string LocalUrl = string.Empty;
                string parametres = rdlcUri + "|" + moduleNmae + "|" + pKey + "|" + printer;
                //int loaderHandler = -1;
                //loaderHandler = LoadingManager.BeginLoading("Impression en cours  ... ");
                WebClient objWebClient = new WebClient();
                int PositionOfClientBin = objWebClient.BaseAddress.ToLower().IndexOf("clientbin");
                if (IsOffline)
                {
                    int taille = objWebClient.BaseAddress.Length;
                    string Suffix = objWebClient.BaseAddress.Substring(PositionOfClientBin, (objWebClient.BaseAddress.Length - PositionOfClientBin));
                    objWebClient.BaseAddress = LocalUrl + Suffix;
                }
                //else
                //{
                //    string Root = objWebClient.BaseAddress.Substring(0, PositionOfClientBin);
                //    string Chaine1 = Root.Replace("http://", " ").Trim();
                //    string[] split = Chaine1.Split(':');
                //    machineName = split[0];

                //    EndpointAddress ep = Utility.EndPoint("Printing");
                //    string adresseserviceImpr = ep.ToString();
                //    string Chaine2 = adresseserviceImpr.Replace("http://", " ").Trim();
                //    string[] split2 = Chaine2.Split('/');
                //    string[] split3 = split2[0].Split(':');
                //    portService = split3[1];


                //}
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        if (resul.Cancelled || resul.Error != null)
                        {
                            //string error = resul.Error.Message;
                            //Message.Show(error,"UploadString =>" +  Galatee.Silverlight.Resources.Langue.errorTitle);
                            //LoadingManager.EndLoading(loaderHandler);
                            return;
                        }
                        //Message.ShowInformation(Galatee.Silverlight.Resources.Langue.confirmPrintSuccess, Galatee.Silverlight.Resources.Langue.informationTitle);
                        //LoadingManager.EndLoading(loaderHandler);
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "objWebClient =>" + Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };

                objWebClient.UploadStringAsync(GetMainURI(rdlcUri, moduleNmae, pKey, printer, portService, machineName, LocalUrl), parametres);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "SendToCaissePrinter =>" + Galatee.Silverlight.Resources.Langue.errorTitle);
            }
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
            int loaderHandler = -1;

            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key + "|" + printer;
                loaderHandler = LoadingManager.BeginLoading(Langue.En_Cours);

                WebClient objWebClient = new WebClient();
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try 
                    {
                        if (resul.Cancelled || resul.Error != null)
                        {
                            string error = resul.Error.Message;
                            Message.Show(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }
                        LoadingManager.EndLoading(loaderHandler);
                    }
                    catch (Exception ex)
                    {
                        LoadingManager.EndLoading(loaderHandler);
                      Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                objWebClient.UploadStringAsync(GetHTTPHandlerURI(rdlcUri, moduleNmae, key, printer), parametres);
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(loaderHandler);
                throw ex;
            }

        }
        static void SendToExport(string NomFichier, string rdlcUri, string moduleNmae, string key, string printer, bool IsLandscape, string Format)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key + "|" + printer + "|" + IsLandscape;
                WebClient objWebClient = new WebClient();
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        if (resul.Cancelled || resul.Error != null)
                        {
                            LoadingManager.EndLoading(res);
                            string error = resul.Error.Message;
                            Message.Show(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }
                        LoadingManager.EndLoading(res);
                    }
                    catch (Exception ex)
                    {
                        LoadingManager.EndLoading(res);
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                objWebClient.UploadStringAsync(GetHTTPHandlerURIExportation(NomFichier, rdlcUri, moduleNmae, key, printer, IsLandscape, Format), parametres);
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }

        }
        static void SendToFichierPlat( string moduleNmae, string key, string printer, string TypeFichier)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {
                string parametres =  moduleNmae + "|" + key + "|" + printer + "|" + TypeFichier;
                WebClient objWebClient = new WebClient();

                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        if (resul.Cancelled || resul.Error != null)
                        {
                            LoadingManager.EndLoading(res);
                            string error = resul.Error.Message;
                            Message.Show(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }
                        LoadingManager.EndLoading(res);
                    }
                    catch (Exception ex)
                    {
                        LoadingManager.EndLoading(res);
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                objWebClient.UploadStringAsync(GetHTTPHandlerFichierPlat(moduleNmae, key, printer, TypeFichier), parametres);
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }
        static void SendToPrinterOrientationTypeEdition(string rdlcUri, string moduleNmae, string key, string printer, bool IsLandscape,string TypeEdition)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key + "|" + printer + "|" + IsLandscape + "|" + TypeEdition;
                WebClient objWebClient = new WebClient();

                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        if (resul.Cancelled || resul.Error != null)
                        {
                            LoadingManager.EndLoading(res);
                            string error = resul.Error.Message;
                            Message.Show(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }
                        LoadingManager.EndLoading(res);
                    }
                    catch (Exception ex)
                    {
                        LoadingManager.EndLoading(res);
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                objWebClient.UploadStringAsync(GetHTTPHandlerURIORIENTATION_TYPEDEMANDE(rdlcUri, moduleNmae, key, printer, IsLandscape,TypeEdition), parametres);
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }
        static void SendToExportTypeEdition(string NomFichier, string rdlcUri, string moduleNmae, string key, string printer, bool IsLandscape,string Format,string TypeEdition)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key + "|" + printer + "|" + IsLandscape + "|"  + TypeEdition;
                WebClient objWebClient = new WebClient();
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        if (resul.Cancelled || resul.Error != null)
                        {
                            LoadingManager.EndLoading(res);
                            string error = resul.Error.Message;
                            Message.Show(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }
                        LoadingManager.EndLoading(res);
                    }
                    catch (Exception ex)
                    {
                        LoadingManager.EndLoading(res);
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                objWebClient.UploadStringAsync(GetHTTPHandlerURIExportation_TYPEDEMANDE(NomFichier,rdlcUri, moduleNmae, key, printer, IsLandscape, Format,TypeEdition), parametres);
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }

        }
        static void SendToPrinterOrientation(string rdlcUri, string moduleNmae, string key, string printer, bool IsLandscape)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);
            //Message.Show(GetHTTPHandlerURIORIENTATION(rdlcUri, moduleNmae, key, printer, IsLandscape) + "  " + parametres, Galatee.Silverlight.Resources.Langue.errorTitle);

            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key + "|" + printer + "|" + IsLandscape;
                WebClient objWebClient = new WebClient();

                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        if (resul.Cancelled || resul.Error != null)
                        {
                            LoadingManager.EndLoading(res);
                            string error = resul.Error.Message;
                            Message.Show(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }
                        LoadingManager.EndLoading(res);
                    }
                    catch (Exception ex)
                    {
                        LoadingManager.EndLoading(res);
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                objWebClient.UploadStringAsync(GetHTTPHandlerURIORIENTATION(rdlcUri, moduleNmae, key, printer, IsLandscape), parametres);
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }
        static void SendToFile(string rdlcUri, string moduleNmae, string key, string printer)
        {
            try
            {
                string Iscreation = true.ToString();
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key + "|" + printer + "|" + Iscreation ;
                int loaderHandler = -1;
                loaderHandler = LoadingManager.BeginLoading("Impression en cours  ... ");
                WebClient objWebClient = new WebClient();

                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        if (resul.Cancelled || resul.Error != null)
                        {
                            string error = resul.Error.Message;
                            Message.Show(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                            LoadingManager.EndLoading(loaderHandler);
                            return;
                        }
                        Message.ShowInformation(Galatee.Silverlight.Resources.Langue.confirmPrintSuccess, Galatee.Silverlight.Resources.Langue.informationTitle);
                        LoadingManager.EndLoading(loaderHandler);
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                objWebClient.UploadStringAsync(GetHTTPHandlerURI(rdlcUri, moduleNmae, key, printer, Iscreation), parametres);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        static void SendToPreviewHandler(string rdlcUri, string moduleNmae, string key)
        {
            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key;
                int loaderHandler = -1;
                loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
                WebClient objWebClient = new WebClient();
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        string keyPreview = resul.Result;

                        if (string.IsNullOrEmpty(keyPreview))
                        {
                            Message.Show(Langue.preview_error_smg, Langue.msg_error_title);
                            return;
                        }

                        // construire le chemin de la page aspx servant d'apercu d'impression
                        string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                        int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
                        string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                        //string urlAspx = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?key={1}", root, keyPreview);

                        LoadingManager.EndLoading(loaderHandler);
                        ShowPreview(root,keyPreview);
                        //ShowPreviewFrame(urlAspx);
                        
                    }
                    catch (Exception ex)
                    {
                        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                        LoadingManager.EndLoading(loaderHandler);
                    }

                };
                objWebClient.UploadStringAsync(GetPreviewHTTPHandlerURI(), parametres);
            }
            catch (Exception ex)
            {

              throw ex;
            }
        }

        static void SendToPreviewHandlerDevis(string rdlcUri, string moduleNmae, string key)
        {
            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key;
                int loaderHandler = -1;
                loaderHandler = LoadingManager.BeginLoading("Operation en cours ... ");
                WebClient objWebClient = new WebClient();
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        string keyPreview = resul.Result;

                        if (string.IsNullOrEmpty(keyPreview))
                        {
                            Message.Show(Langue.preview_error_smg, Langue.msg_error_title);
                            return;
                        }

                        // construire le chemin de la page aspx servant d'apercu d'impression
                        string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                        int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
                        string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                        //string urlAspx = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?key={1}", root, keyPreview);

                        LoadingManager.EndLoading(loaderHandler);
                        ShowPreviewDevis(root, keyPreview);
                        //ShowPreviewFrame(urlAspx);

                    }
                    catch (Exception ex)
                    {
                        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                        LoadingManager.EndLoading(loaderHandler);
                    }

                };
                objWebClient.UploadStringAsync(GetPreviewHTTPHandlerURI(), parametres);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        static void SendToMailHandler(string rdlcUri, string moduleNmae, string key)
        {
            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key;
                int loaderHandler = -1;
                WebClient objWebClient = new WebClient();
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        string keyPreview = resul.Result;

                        if (string.IsNullOrEmpty(keyPreview))
                        {
                            //Message.Show(Langue.preview_error_smg, Langue.msg_error_title);
                            return;
                        }

                        // construire le chemin de la page aspx servant d'apercu d'impression
                        string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                        int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
                        string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                        //string urlAspx = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?key={1}", root, keyPreview);

                        //ShowPreview(root,keyPreview);
                        //ShowPreviewFrame(urlAspx);
                        
                    }
                    catch (Exception ex)
                    {
                        //Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }

                };
                objWebClient.UploadStringAsync(GetPreviewHTTPHandlerURIForEmail(), parametres);
            }
            catch (Exception ex)
            {

              throw ex;
            }
        }
        //static void SendToSmsHandler(string MessageSms, string Numero)
        //{
        //    try
        //    {
        //        string parametres = MessageSms + "|" + Numero;
        //        int loaderHandler = -1;
        //        loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
        //        WebClient objWebClient = new WebClient();
        //        objWebClient.UploadStringCompleted += (send, resul) =>
        //        {
        //            try
        //            {
        //                string keyPreview = resul.Result;

        //                if (string.IsNullOrWhiteSpace(keyPreview))
        //                {
        //                    Message.Show("Un problème est survenue lors de l'envoi des sms", Langue.msg_error_title);
        //                    LoadingManager.EndLoading(loaderHandler);
        //                    return;
        //                }
                        
        //                Message.Show("l'envoi des sms c'est passé avec succès", Langue.msg_error_title);
        //                LoadingManager.EndLoading(loaderHandler);
        //                return;
        //            }
        //            catch (Exception ex)
        //            {
        //                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
        //                LoadingManager.EndLoading(loaderHandler);
        //            }

        //        };
        //        objWebClient.UploadStringAsync(GetHTTPHandlerURIForSms(), parametres);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        public static void SendToHandlerCaisse(string rdlcUri, string moduleNmae, string key)
        {
            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key;
                int loaderHandler = -1;
                loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
                WebClient objWebClient = new WebClient();
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        string keyPreview = resul.Result;

                        if (string.IsNullOrEmpty(keyPreview))
                        {
                            Message.Show(Langue.preview_error_smg, Langue.msg_error_title);
                            return;
                        }

                        // construire le chemin de la page aspx servant d'apercu d'impression
                        string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                        int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
                        string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                        //string urlAspx = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?key={1}", root, keyPreview);

                        LoadingManager.EndLoading(loaderHandler);
                        ShowPreviewCaisse(root, keyPreview);
                        //ShowPreviewFrame(urlAspx);

                    }
                    catch (Exception ex)
                    {
                        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                        LoadingManager.EndLoading(loaderHandler);
                    }

                };
                objWebClient.UploadStringAsync(GetPreviewHTTPHandlerURI(), parametres);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public  static void SendToHandler(string rdlcUri, string moduleNmae, string key)
        {
            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key + "|" +SessionObject.ServerEndPointName + "|" + SessionObject.ServerEndPointPort;
                int loaderHandler = -1;
                loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
                WebClient objWebClient = new WebClient();
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        string keyPreview = resul.Result;
                        if (string.IsNullOrEmpty(keyPreview))
                        {
                            Message.Show(Langue.preview_error_smg, Langue.msg_error_title);
                            return;
                        }

                        // construire le chemin de la page aspx servant d'apercu d'impression
                        string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                        int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
                        string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                        //string urlAspx = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?key={1}", root, keyPreview);

                        LoadingManager.EndLoading(loaderHandler);
                        ShowPreview(root, keyPreview);
                        //ShowPreviewFrame(urlAspx);

                    }
                    catch (Exception ex)
                    {
                        LoadingManager.EndLoading(loaderHandler);
                        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                        
                    }

                };
                objWebClient.UploadStringAsync(GetPreviewHTTPHandlerURI(), parametres);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void SendToHandlerAccueil(string rdlcUri, string moduleNmae, string key)
        {
            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key;
                int loaderHandler = -1;
                loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
                WebClient objWebClient = new WebClient();
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        string keyPreview = resul.Result;

                        if (string.IsNullOrEmpty(keyPreview))
                        {
                            Message.Show(Langue.preview_error_smg, Langue.msg_error_title);
                            return;
                        }

                        // construire le chemin de la page aspx servant d'apercu d'impression
                        string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                        int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
                        string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                        //string urlAspx = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?key={1}", root, keyPreview);

                        LoadingManager.EndLoading(loaderHandler);
                        ShowPreviewDevis(root, keyPreview);
                        //ShowPreviewFrame(urlAspx);

                    }
                    catch (Exception ex)
                    {
                        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                        LoadingManager.EndLoading(loaderHandler);
                    }

                };
                objWebClient.UploadStringAsync(GetPreviewHTTPHandlerURI(), parametres);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static bool ShowPreviewFrame(string Control,string module)
        {
            try 
	            {
                    string hauteur = string.Empty;
		            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                    int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
                    string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                    string matricule = UserConnecte.matricule;
                    string URI = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?ControlName={1}&Module={2}&Matricule={3}", root, Control, module, matricule);

                    string currentmodule = SessionObject.moduleCourant;
                    HtmlElement DivFrame = HtmlPage.Document.GetElementById(currentmodule + "DIV_IFRAME");
                    HtmlElement Frame = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME");
                    //HtmlElement CloseButton = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME_INPUT");
                
                    if (DivFrame != null)
                    {
                        DivFrame.SetStyleAttribute("width", "100%");
                        DivFrame.SetStyleAttribute("height", "100%");
                        DivFrame.SetStyleAttribute("z-index", "25");

                        //CloseButton.SetStyleAttribute("width", "3px");
                        //CloseButton.SetStyleAttribute("height", "2px");
                        //CloseButton.SetStyleAttribute("visibility", "visible");

                        Frame.SetStyleAttribute("width", "90%");
                        Frame.SetStyleAttribute("height", "90%");
                        Frame.SetStyleAttribute("visibility", "visible");
                        Frame.SetAttribute("src", URI);

                        //PluginFrame.SetAttribute("src", "http://localhost:17207/Galatee.ModuleLoaderTestPage.aspx");
                    }
                   return true;
	            }
	            catch (Exception ex)
	            {
		            throw ex;
	            }

        }

        static void ShowPreviewFrame(string URI)
        {

            try
            {
                string hauteur = string.Empty;
                string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
                string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                string matricule = UserConnecte.matricule;
                //string URI = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?ControlName={1}&Module={2}&Matricule={3}", root, Control, module, matricule);

                string currentmodule = SessionObject.moduleCourant;
                HtmlElement DivFrame = HtmlPage.Document.GetElementById(currentmodule + "DIV_IFRAME");
                HtmlElement Frame = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME");

                if (DivFrame != null)
                {
                    DivFrame.SetStyleAttribute("width", "100%");
                    DivFrame.SetStyleAttribute("height", "100%");
                    DivFrame.SetStyleAttribute("z-index", "25");
                    Frame.SetStyleAttribute("width", "100%");
                    Frame.SetStyleAttribute("height", "100%");
                    Frame.SetStyleAttribute("visibility", "visible");
                    Frame.SetAttribute("src", URI);
                    //PluginFrame.SetAttribute("src", "http://localhost:17207/Galatee.ModuleLoaderTestPage.aspx");
                }
                //return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //string currentmodule = SessionObject.moduleCourant;
            //HtmlElement pgl = HtmlPage.Document.GetElementById(currentmodule + "pgl");
            //HtmlElement PluginFrame = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME");
            //HtmlElement BUTTONFRAME = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME_BUTTON");

            //if (pgl != null)
            //{
            //    pgl.SetStyleAttribute("width", "100%");
            //    pgl.SetStyleAttribute("height", "20%");
            //    pgl.SetStyleAttribute("top", "0");
            //    pgl.SetStyleAttribute("visibility", "visible");
            //}

            //if (PluginFrame != null)
            //{
            //    PluginFrame.SetStyleAttribute("width", "100%");
            //    PluginFrame.SetStyleAttribute("height", "50%");
            //    PluginFrame.SetStyleAttribute("left", "0");
            //    PluginFrame.SetStyleAttribute("bottom", "10px");
            //    PluginFrame.SetStyleAttribute("visibility", "visible");
            //    PluginFrame.SetAttribute("src", URI);
            //    //PluginFrame.SetAttribute("src", "http://localhost:17207/Galatee.ModuleLoaderTestPage.aspx");

            //    BUTTONFRAME.SetStyleAttribute("visibility", "visible");


            //}

            // modification apporter par le designer

            //string currentmodule = SessionObject.moduleCourant;
            //HtmlElement pgl = HtmlPage.Document.GetElementById(currentmodule + "pgl");
            //HtmlElement PluginFrame = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME");
            //HtmlElement DIVFRAME = HtmlPage.Document.GetElementById(currentmodule + "DIV_IFRAME");

            //if (pgl != null)
            //{
            //    pgl.SetStyleAttribute("width", "100%");
            //    pgl.SetStyleAttribute("height", "20%");
            //    pgl.SetStyleAttribute("top", "0");
            //    pgl.SetStyleAttribute("visibility", "visible");
            //}

            //if (PluginFrame != null)
            //{
            //    PluginFrame.SetStyleAttribute("width", "100%");
            //    PluginFrame.SetStyleAttribute("height", "20%");
            //    PluginFrame.SetStyleAttribute("left", "0");
            //    PluginFrame.SetStyleAttribute("bottom", "10px");
            //    PluginFrame.SetStyleAttribute("visibility", "visible");
            //    PluginFrame.SetAttribute("src", URI);
            //    //PluginFrame.SetAttribute("src", "http://localhost:17207/Galatee.ModuleLoaderTestPage.aspx");
            //    DIVFRAME.SetStyleAttribute("height", "20%");

            //    //BUTTONFRAME.SetStyleAttribute("visibility", "visible");


            //}
        }
        static void ShowPreviewCaisse(string pRoot, string key)
        {

            try
            {
                string hauteur = string.Empty;
                string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
                string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                string matricule = UserConnecte.matricule;
                //string URI = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?ControlName={1}&Module={2}&Matricule={3}", root, Control, module, matricule);

                string currentmodule = SessionObject.moduleCourant;
                HtmlElement DivFrame = HtmlPage.Document.GetElementById(currentmodule + "DIV_IFRAME");
                HtmlElement Frame = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME");
                HtmlElement CloseButton = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME_INPUT");


                string urlAspx = String.Format(@"{0}" + "HandlerPrinting/PreviewPrintingCaisse.aspx?key={1}&frame={2}", pRoot, key, currentmodule + "DIV_IFRAME");

                string visibile = DivFrame.GetStyleAttribute("display");
                if (visibile != null && visibile.Equals("none"))
                    DivFrame.SetStyleAttribute("display", "block");

                if (DivFrame != null)
                {
                    DivFrame.SetStyleAttribute("width", "100%");
                    DivFrame.SetStyleAttribute("height", "100%");
                    DivFrame.SetStyleAttribute("z-index", "25");

                    CloseButton.SetStyleAttribute("width", "10px");
                    CloseButton.SetStyleAttribute("height", "5px");
                    CloseButton.SetStyleAttribute("visibility", "visible");

                    Frame.SetStyleAttribute("width", "100%");
                    Frame.SetStyleAttribute("height", "100%");
                    Frame.SetStyleAttribute("visibility", "visible");
                    Frame.SetAttribute("src", urlAspx);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void ShowPreviewAccueil(string pRoot, string key)
        {

            try
            {
                string hauteur = string.Empty;
                string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
                string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                string matricule = UserConnecte.matricule;
                //string URI = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?ControlName={1}&Module={2}&Matricule={3}", root, Control, module, matricule);

                string currentmodule = SessionObject.moduleCourant;
                HtmlElement DivFrame = HtmlPage.Document.GetElementById(currentmodule + "DIV_IFRAME");
                HtmlElement Frame = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME");
                HtmlElement CloseButton = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME_INPUT");


                string urlAspx = String.Format(@"{0}" + "HandlerPrinting/PreviewPrintingCaisse.aspx?key={1}&frame={2}", pRoot, key, currentmodule + "DIV_IFRAME");

                string visibile = DivFrame.GetStyleAttribute("display");
                if (visibile != null && visibile.Equals("none"))
                    DivFrame.SetStyleAttribute("display", "block");

                if (DivFrame != null)
                {
                    DivFrame.SetStyleAttribute("width", "100%");
                    DivFrame.SetStyleAttribute("height", "100%");
                    DivFrame.SetStyleAttribute("z-index", "25");

                    CloseButton.SetStyleAttribute("width", "10px");
                    CloseButton.SetStyleAttribute("height", "5px");
                    CloseButton.SetStyleAttribute("visibility", "visible");

                    Frame.SetStyleAttribute("width", "100%");
                    Frame.SetStyleAttribute("height", "100%");
                    Frame.SetStyleAttribute("visibility", "visible");
                    Frame.SetAttribute("src", urlAspx);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void ShowPreview(string pRoot,string key)
        {

            try
            {
                string matricule = UserConnecte.matricule;
                //string URI = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?ControlName={1}&Module={2}&Matricule={3}", root, Control, module, matricule);

                string currentmodule = SessionObject.moduleCourant;
                HtmlElement DivFrame = HtmlPage.Document.GetElementById(currentmodule + "DIV_IFRAME");
                HtmlElement Frame = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME");
                HtmlElement CloseButton = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME_INPUT");
                string urlAspx = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting" +currentmodule + ".aspx?key={1}&frame={2}", pRoot, key, currentmodule + "DIV_IFRAME");

                Message.ShowInformation(urlAspx, "CheminFichierHandler");
                 string visibile = DivFrame.GetStyleAttribute("display");
                 if (visibile != null && visibile.Equals("none"))
                     DivFrame.SetStyleAttribute("display","block");

                if (DivFrame == null )
                    Message.ShowInformation("Div", "Pas de frame");


                if (DivFrame != null)
                {
                    Message.ShowInformation("Div", "frame trouve");

                    DivFrame.SetStyleAttribute("width", "100%");
                    DivFrame.SetStyleAttribute("height", "100%");
                    DivFrame.SetStyleAttribute("z-index", "25");

                    CloseButton.SetStyleAttribute("width", "10px");
                    CloseButton.SetStyleAttribute("height", "5px");
                    CloseButton.SetStyleAttribute("visibility", "visible");

                    Frame.SetStyleAttribute("width", "100%");
                    Frame.SetStyleAttribute("height", "100%");
                    Frame.SetStyleAttribute("visibility", "visible");

                    Frame.SetAttribute("src", urlAspx);
                    Message.ShowInformation("Div", "Terminer");

                    //PluginFrame.SetAttribute("src", "http://localhost:17207/Galatee.ModuleLoaderTestPage.aspx");
                }
                //return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //string currentmodule = SessionObject.moduleCourant;
            //HtmlElement pgl = HtmlPage.Document.GetElementById(currentmodule + "pgl");
            //HtmlElement PluginFrame = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME");
            //HtmlElement BUTTONFRAME = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME_BUTTON");

            //if (pgl != null)
            //{
            //    pgl.SetStyleAttribute("width", "100%");
            //    pgl.SetStyleAttribute("height", "20%");
            //    pgl.SetStyleAttribute("top", "0");
            //    pgl.SetStyleAttribute("visibility", "visible");
            //}

            //if (PluginFrame != null)
            //{
            //    PluginFrame.SetStyleAttribute("width", "100%");
            //    PluginFrame.SetStyleAttribute("height", "50%");
            //    PluginFrame.SetStyleAttribute("left", "0");
            //    PluginFrame.SetStyleAttribute("bottom", "10px");
            //    PluginFrame.SetStyleAttribute("visibility", "visible");
            //    PluginFrame.SetAttribute("src", URI);
            //    //PluginFrame.SetAttribute("src", "http://localhost:17207/Galatee.ModuleLoaderTestPage.aspx");

            //    BUTTONFRAME.SetStyleAttribute("visibility", "visible");


            //}

            // modification apporter par le designer

            //string currentmodule = SessionObject.moduleCourant;
            //HtmlElement pgl = HtmlPage.Document.GetElementById(currentmodule + "pgl");
            //HtmlElement PluginFrame = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME");
            //HtmlElement DIVFRAME = HtmlPage.Document.GetElementById(currentmodule + "DIV_IFRAME");

            //if (pgl != null)
            //{
            //    pgl.SetStyleAttribute("width", "100%");
            //    pgl.SetStyleAttribute("height", "20%");
            //    pgl.SetStyleAttribute("top", "0");
            //    pgl.SetStyleAttribute("visibility", "visible");
            //}

            //if (PluginFrame != null)
            //{
            //    PluginFrame.SetStyleAttribute("width", "100%");
            //    PluginFrame.SetStyleAttribute("height", "20%");
            //    PluginFrame.SetStyleAttribute("left", "0");
            //    PluginFrame.SetStyleAttribute("bottom", "10px");
            //    PluginFrame.SetStyleAttribute("visibility", "visible");
            //    PluginFrame.SetAttribute("src", URI);
            //    //PluginFrame.SetAttribute("src", "http://localhost:17207/Galatee.ModuleLoaderTestPage.aspx");
            //    DIVFRAME.SetStyleAttribute("height", "20%");

            //    //BUTTONFRAME.SetStyleAttribute("visibility", "visible");


            //}
        }

        static void ShowPreviewDevis(string pRoot, string key)
        {

            try
            {
                string hauteur = string.Empty;
                string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
                string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                string matricule = UserConnecte.matricule;
                //string URI = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?ControlName={1}&Module={2}&Matricule={3}", root, Control, module, matricule);

                string currentmodule = SessionObject.moduleCourant;
                HtmlElement DivFrame = HtmlPage.Document.GetElementById(currentmodule + "DIV_IFRAME");
                HtmlElement Frame = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME");
                HtmlElement CloseButton = HtmlPage.Document.GetElementById(currentmodule + "_IFRAME_INPUT");


                string urlAspx = String.Format(@"{0}" + "HandlerPrinting/PreviewPrintingDevis.aspx?key={1}&frame={2}", pRoot, key, currentmodule + "DIV_IFRAME");

                string visibile = DivFrame.GetStyleAttribute("display");
                if (visibile != null && visibile.Equals("none"))
                    DivFrame.SetStyleAttribute("display", "block");

                if (DivFrame != null)
                {
                    DivFrame.SetStyleAttribute("width", "100%");
                    DivFrame.SetStyleAttribute("height", "100%");
                    DivFrame.SetStyleAttribute("z-index", "25");

                    CloseButton.SetStyleAttribute("width", "10px");
                    CloseButton.SetStyleAttribute("height", "5px");
                    CloseButton.SetStyleAttribute("visibility", "visible");

                    Frame.SetStyleAttribute("width", "100%");
                    Frame.SetStyleAttribute("height", "100%");
                    Frame.SetStyleAttribute("visibility", "visible");
                    Frame.SetAttribute("src", urlAspx);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);

            //string url = "ConnexionsElectricite";
            Uri UriWebService = new Uri(String.Format(@"{0}" + "ReportAspPage/" + moduleName + @"/" + url + ".ashx", root));

            return UriWebService;
        }

        /// <summary>
        /// Permet de determiner l'url du fichier http handler  sur le serveur web
        /// NB ce fichier est le point d'entree de toute requete d'impression vers le projet silverlight web
        /// NOM DU FICHIER HANDLER : GenericPrintHandler.ashx
        /// </summary>
        /// <returns></returns>
        static public Uri GetHTTPHandlerURI()
        {
            string handler = "GenericPrintHandler.ashx";
            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);

            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler, root));

            return UriWebService;
        }
        static public Uri GetHTTPHandlerURIORIENTATION(string rdlcName, string moduleName, string key, string printer, bool IsLandscape)
        {
            string handler = "Main";
            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}&printer={4}&IsLandscape={5}&machine={6}&port={7}", root, rdlcName, moduleName, key, printer, IsLandscape, SessionObject.ServerEndPointName, SessionObject.ServerEndPointPort));
            return UriWebService;
        }
        static public Uri GetHTTPHandlerURIORIENTATION_TYPEDEMANDE(string rdlcName, string moduleName, string key, string printer, bool IsLandscape, string TypeEdition)
        {
            string handler = "Main";
            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}&printer={4}&IsLandscape={5}&machine={6}&port={7}&TypeEdition={8}", root, rdlcName, moduleName, key, printer, IsLandscape, SessionObject.ServerEndPointName, SessionObject.ServerEndPointPort,TypeEdition));
            return UriWebService;
        }
        static public Uri GetHTTPHandlerURIExportation_TYPEDEMANDE(string NomFichier, string rdlcName, string moduleName, string key, string printer, bool IsLandscape,string Format, string TypeEdition)
        {
            string handler = "Main";
            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}&printer={4}&IsLandscape={5}&IsExport={6}&FormatExport={7}&machine={8}&port={9}&NomDuFichier={10}&TypeEdition={11}", root, rdlcName, moduleName, key, printer, IsLandscape, true, Format, SessionObject.ServerEndPointName, SessionObject.ServerEndPointPort, NomFichier, TypeEdition));
            return UriWebService;
        }
        static public Uri GetHTTPHandlerURIExportation(string NomFichier, string rdlcName, string moduleName, string key, string printer, bool IsLandscape,string Format)
        {
            string handler = "Main";
            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}&printer={4}&IsLandscape={5}&IsExport={6}&FormatExport={7}&machine={8}&port={9}&NomDuFichier={10}", root, rdlcName, moduleName, key, printer, IsLandscape, true, Format, SessionObject.ServerEndPointName, SessionObject.ServerEndPointPort, NomFichier));
            return UriWebService;
        }
        static public Uri GetHTTPHandlerURI(string rdlcName, string moduleName, string key, string printer, string IscreationOnly)
        {
            string handler = "Main";
            //string handler = "GenericPrintHandler.ashx";
            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
            //Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}", root, rdlcName, moduleName, key));
            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}&printer={4}&IscreationOnly={5}", root, rdlcName, moduleName, key, printer, IscreationOnly));
            //Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler, root));
            //MessageBox.Show(strBaseWebAddress);
            return UriWebService;
        }
        static public Uri GetHTTPHandlerURI(string rdlcName, string moduleName, string key, string printer)
        {
            string handler = "Main";
            //string handler = "GenericPrintHandler.ashx";
            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
            //Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}", root, rdlcName, moduleName, key));
            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}&printer={4}", root, rdlcName, moduleName, key, printer));
            //Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler, root));
            //MessageBox.Show(strBaseWebAddress);
            return UriWebService;
        }
        static public Uri GetHTTPHandlerFichierPlat(string moduleName, string key, string printer, string TypeFchier)
        {
            string handler = "Main";
            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?module={1}&key={2}&printer={3}&TypeFchier={4}&machine={5}&port={6}", root, moduleName, key, printer, TypeFchier, SessionObject.ServerEndPointName, SessionObject.ServerEndPointPort));
            return UriWebService;
        }
        static public Uri GetMainURI(string rdlcName, string moduleName, string key, string printer, string port, string machineName,string LocalRoot)
        {
            try
            {


                string lePrinter = printer.Replace("\\", "9");
                printer = lePrinter;
                string handler = "Main";
                string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
                string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                root = string.IsNullOrEmpty(LocalRoot) ? strBaseWebAddress.Substring(0, PositionOfClientBin) : LocalRoot;
                Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}&printer={4}&port={5}&machine={6}", root, rdlcName, moduleName, key, printer, port, machineName));
                return UriWebService;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public Uri GetMainURI(string rdlcName, string moduleName, string key, string printer, string port, string machineName)
        {
            try
            {
                string handler = "Main";
                //string handler = "GenericPrintHandler.ashx";
                string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
                string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}&printer={4}&port={5}&machine={6}", root, rdlcName, moduleName, key, printer, port, machineName));
                return UriWebService;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static public Uri GetPreviewHTTPHandlerURI()
        {
            string handler = "GenericPrintPreviewHandler.ashx";
            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                //+ "|" + SessionObject.ServerEndPointName + "|" + SessionObject.ServerEndPointPort;

            //Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler, root));
            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".ashx?root={1}&machine={2}&port={3}", root,SessionObject.ServerEndPointName, SessionObject.ServerEndPointPort));

            return UriWebService;
        }
        static public Uri GetPreviewHTTPHandlerURIForEmail()
        {
            string handler = "MailFactures.ashx";
            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);

            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler, root));

            return UriWebService;
        }
        static public Uri GetHTTPHandlerURIForSms()
        {
            string handler = "SmsHandler.ashx";
            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);

            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler, root));

            return UriWebService;
        }
        static public string GetHTTPWebAppBaseAddresse()
        {
            //string handler = "SmsHandler.ashx";
            string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            string root = strBaseWebAddress.Substring(0, PositionOfClientBin);

            //Uri UriWebService = new Uri(String.Format(@"{0}" , root));

            //return UriWebService;
            return root;
        }

        /// <summary>
        /// Permet de determiner le chemin du Rapport .rdlc a imprimé
        /// à partir d'un projet silverlight
        /// </summary>
        /// <param name="url">nom du fichier .rdlc a imprimé</param>
        /// <param name="moduleName">nom du module dans lequel ce trouve le rapport .rdlc</param>
        /// <param name="key">cle d'invocation de la page web</param>
        /// <returns></returns>
        static Uri GetBaseAddress(string rdlcName, string moduleName, string key)
        {
            try
            {
                string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf(@"clientbin");
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
        /// <param name="param"></param>
        /// <returns></returns>
        public static string setParam(string param)
        {
            try
            {
                string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf(@"clientbin");
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
        /// NB 
        /// </summary>
        /// <returns></returns>
        public static string getKey()
        {
            try
            {
                //string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                //int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf(@"clientbin");
                //string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                //string clientIpAddrss = root.Split(':')[1].Split('/')[2];
                //return clientIpAddrss;
                string matricule = UserConnecte.matricule;
                string heure = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                return matricule + heure;
                //return SessionObject.MachineName;
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
        /// NB Les types T et P doivent avoir le meme nombre de propriétés de meme type 
        /// </summary>
        /// <typeparam name="T">Type de retour</typeparam>
        /// <typeparam name="P">Type de destination</typeparam>
        /// <param name="ListeDobjects">Liste source</param>
        /// <returns></returns>
        public static List<T> ConvertListType<T, P>(List<P> ListeDobjects) where T : new()
        {
            try
            {
                // Parsing de originalArray vers Arrays
                List<T> ListeAImprimer = new List<T>();
                //for (int objNum = 0; objNum < ListeDobjects.Count; objNum++)
                T objVide = new T();
                foreach (P item in ListeDobjects)
                {
                    if (item != null)
                    {
                        objVide = Utility.ParseObject<T, P>(objVide, item);
                        ListeAImprimer.Add(objVide);
                        objVide = new T();
                    }
                }
                return ListeAImprimer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public static  T ConvertType<T, P>(P ListeDobjects) where T : new()
        {
            try
            {
                // Parsing de originalArray vers Arrays
                //for (int objNum = 0; objNum < ListeDobjects.Count; objNum++)
                T objVide = new T();
                objVide = Utility.ParseObject<T, P>(objVide, ListeDobjects);
                return objVide;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new T();
            }
        }

        internal static void SendToSmsHandler(string MessageSms, string Numero)
        {
            try
            {
                string parametres = MessageSms + "|" + Numero;
                int loaderHandler = -1;
                loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
                WebClient objWebClient = new WebClient();
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        string keyPreview = resul.Result;

                        if (string.IsNullOrWhiteSpace(keyPreview))
                        {
                            Message.Show("Un problème est survenue lors de l'envoi des sms", Langue.msg_error_title);
                            LoadingManager.EndLoading(loaderHandler);
                            return;
                        }

                        Message.Show("l'envoi des sms c'est passé avec succès", Langue.msg_error_title);
                        LoadingManager.EndLoading(loaderHandler);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                        LoadingManager.EndLoading(loaderHandler);
                    }

                };
                objWebClient.UploadStringAsync(GetHTTPHandlerURIForSms(), parametres);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
   
    }

}