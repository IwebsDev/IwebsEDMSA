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
using System.Collections.ObjectModel;
//using Galatee.Silverlight.serviceWeb;
//using Galatee.Silverlight.ServiceCaisse;
using System.Windows.Browser;
using System.Xml.Serialization;
using System.Text;
using System.Xml;

namespace Galatee.Silverlight
{
    static public class Prints
    {
        /// <summary>
        ///  Permet de lancer l'impression a partir d'un projet silverlight.
        ///  <br>L'impression s'execute cote server web</br>
        ///  
        /// </summary>
        /// <param name="objects">Liste d'objets servant à l'impression de etats</param>
        /// <param name="param">dictionnaire des parametres du rapport</param>
        /// <param name="rdlcUri">nom du fichier .rdlc a imprimé</param>
        /// <param name="moduleNmae">nom du module dans lequel se trouve le rapport( nom du repertoire)</param>
        /// <param name="key">cle automatique d'invocation de la page web</param>
        //static public void Action(List<CsReglement> objects, Dictionary<string, string> param, string rdlcUri, string moduleNmae, string key)
        //{
        //    List<CsReglement> arrays = new List<CsReglement>();
        //    //ArrayOfAnyType arrays = new ArrayOfAnyType();
        //    foreach (CsReglement o in objects)
        //        arrays.Add(o);
        //    Service1Client client = new Service1Client();
        //    client.setReglementInWebPartCompleted += (sender, args) =>
        //        {
        //            if (args.Result == true)
        //            {
        //                HtmlPopupWindowOptions opt = new HtmlPopupWindowOptions();
        //                opt.Width = 1;
        //                opt.Height = 1;
        //                HtmlPage.Window.Navigate(GetBaseAddress(rdlcUri, moduleNmae, key), "_blank", "width=1,height=1");
        //            }
        //        };
        //    client.setReglementInWebPartAsync(key, arrays,param);
        //}

        //static public void Action(List<CsDetailMoratoire> objects, Dictionary<string, string> param, string rdlcUri, string moduleNmae, string key)
        //{
        //    List<CsDetailMoratoire> arrays = new List<CsDetailMoratoire>();
        //    foreach (CsDetailMoratoire o in objects)
        //        arrays.Add(o);
        //    Service1Client client = new Service1Client();
        //    client.setDetailMoratoireInWebPartCompleted += (sender, args) =>
        //    {
        //        if (args.Error != null || args.Cancelled || !args.Result)
        //        {
        //            Message.Show("error on remote call for printing miscallenous", Galatee.Silverlight.Resources.Langue.wcf_error);
        //            string error = args.Error.Message;

        //            // on doit pouvoir supprimer le moratoire précédent pour l'integriter des données
        //            return;
        //        }
               
        //            HtmlPopupWindowOptions opt = new HtmlPopupWindowOptions();
        //            opt.Width = 1;
        //            opt.Height = 1;
        //            HtmlPage.Window.Navigate(GetBaseAddress(rdlcUri, moduleNmae, key), "_blank", "width=1,height=1");
                    
                   
                    
        //    };
        //    client.setDetailMoratoireInWebPartAsync(key, arrays, param);
        //}

        //static public void Action(List<aCampagne> objects, Dictionary<string, string> param, string rdlcUri, string moduleNmae, string key)
        //{
        //    List<aCampagne> arrays = new List<aCampagne>();
        //    foreach (aCampagne o in objects)
        //        arrays.Add(o);
        //    Service1Client client = new Service1Client();
        //    client.setDetailCampagneInWebPartCompleted += (sender, args) =>
        //    {
        //        if (args.Error != null || args.Cancelled || !args.Result)
        //        {
        //            Message.Show("error on remote call for printing miscallenous", Galatee.Silverlight.Resources.Langue.wcf_error);
        //            string error = args.Error.Message;

        //            // on doit pouvoir supprimer le moratoire précédent pour l'integriter des données
        //            return;
        //        }

        //        HtmlPopupWindowOptions opt = new HtmlPopupWindowOptions();
        //        opt.Width = 1;
        //        opt.Height = 1;
        //        HtmlPage.Window.Navigate(GetBaseAddress(rdlcUri, moduleNmae, key), "_blank", "width=1,height=1");



        //    };
        //    client.setDetailCampagneInWebPartAsync(key, arrays, param);
        //}

        //static public void Action(List<aDisconnection> objects, Dictionary<string, string> param, string rdlcUri, string moduleNmae, string key)
        //{
        //    List<aDisconnection> arrays = new List<aDisconnection>();
        //    foreach (aDisconnection o in objects)
        //        arrays.Add(o);
        //    Service1Client client = new Service1Client();
        //    client.setDetailDsiconnectionInWebPartCompleted += (sender, args) =>
        //    {
        //        if (args.Error != null || args.Cancelled || !args.Result)
        //        {
        //            Message.Show("error on remote call for printing miscallenous", Galatee.Silverlight.Resources.Langue.wcf_error);
        //            string error = args.Error.Message;

        //            // on doit pouvoir supprimer le moratoire précédent pour l'integriter des données
        //            return;
        //        }

        //        HtmlPopupWindowOptions opt = new HtmlPopupWindowOptions();
        //        opt.Width = 1;
        //        opt.Height = 1;
        //        HtmlPage.Window.Navigate(GetBaseAddress(rdlcUri, moduleNmae, key), "_blank", "width=1,height=1");

        //    };
        //    client.setDetailDsiconnectionInWebPartAsync(key, arrays, param);
        //}

        //static public void Action(List<aMoratoire> objects, Dictionary<string, string> param, string rdlcUri, string moduleNmae, string key)
        //{
        //    List<aMoratoire> arrays = new List<aMoratoire>();
        //    foreach (aMoratoire o in objects)
        //        arrays.Add(o);
        //    Service1Client client = new Service1Client();
        //    client.setDetailaMoratoireInWebPartCompleted += (sender, args) =>
        //    {
        //        if (args.Error != null || args.Cancelled || !args.Result)
        //        {
        //            Message.Show("error on remote call for printing miscallenous", Galatee.Silverlight.Resources.Langue.wcf_error);
        //            string error = args.Error.Message;

        //            // on doit pouvoir supprimer le moratoire précédent pour l'integriter des données
        //            return;
        //        }

        //        HtmlPopupWindowOptions opt = new HtmlPopupWindowOptions();
        //        opt.Width = 1;
        //        opt.Height = 1;
        //        HtmlPage.Window.Navigate(GetBaseAddress(rdlcUri, moduleNmae, key), "_blank", "width=1,height=1");

        //    };
        //    client.setDetailaMoratoireInWebPartAsync(key, arrays, param);
        //}

        //static public void Action(List<aRdv> objects, Dictionary<string, string> param, string rdlcUri, string moduleNmae, string key)
        //{
        //    List<aRdv> arrays = new List<aRdv>();
        //    foreach (aRdv o in objects)
        //        arrays.Add(o);
        //    Service1Client client = new Service1Client();
        //    client.setDetailaRdvInWebPartCompleted += (sender, args) =>
        //    {
        //        if (args.Error != null || args.Cancelled || !args.Result)
        //        {
        //            Message.Show("error on remote call for printing miscallenous", Galatee.Silverlight.Resources.Langue.w);
        //            Message.Show("error on remote call for printing miscallenous",);
        //            string error = args.Error.Message;

        //            // on doit pouvoir supprimer le moratoire précédent pour l'integriter des données
        //            return;
        //        }

        //        HtmlPopupWindowOptions opt = new HtmlPopupWindowOptions();
        //        opt.Width = 1;
        //        opt.Height = 1;
        //        HtmlPage.Window.Navigate(GetBaseAddress(rdlcUri, moduleNmae, key), "_blank", "width=1,height=1");

        //    };
        //    client.setDetailaRdvInWebPartAsync(key, arrays, param);
        //}

       //static public void Actions<T>(List<T> objects, string rdlcName, string moduleNmae, string key)
       //{
       //    var s = new XmlSerializer(typeof(List<T>));
       //    StringBuilder sb = new StringBuilder();
       //    XmlWriter wr = XmlWriter.Create(sb);
       //    s.Serialize(wr, objects);

       //    string keys = string.Format("{0}", sb.ToString());
       //    int index = objects[0].GetType().ToString().LastIndexOf(".");
       //    string type= objects[0].GetType().ToString().Substring(index+1);
       //    printingserviceSoapClient client =new printingserviceSoapClient();
       //    client.setNameCompleted+= (sender, args) =>
       //    {
       //        if (args.Result == true)
       //        {
       //            HtmlPopupWindowOptions opt = new HtmlPopupWindowOptions();
       //            opt.Width = 1;
       //            opt.Height = 1;
       //            HtmlPage.Window.Navigate(GetBaseAddress(rdlcName, moduleNmae, key), "_blank", "width=1,height=1");
       //        }
       //    };
       //    client.setNameAsync(keys, type);
       //}

        /// <summary>
        /// Permet de determiner le chemin du Rapport .rdlc a imprimé
        /// à partir d'un projet silverlight
        /// </summary>
        /// <param name="url">nom du fichier .rdlc a imprimé</param>
       /// <param name="moduleName">nom du module dans lequel ce trouve le rapport .rdlc</param>
       /// <param name="key">cle d'invocation de la page web</param>
        /// <returns></returns>
       static Uri GetBaseAddress(string rdlcName,string moduleName, string key)
        {
            try
            {
                string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf(@"clientbin");
                string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                Uri UriWebService = new Uri(String.Format(@"{0}" + "ReportAspPage/" + moduleName+@"/"+rdlcName + ".aspx?key={1}&rdlc={2}&module={3}", root, key, rdlcName, moduleName));
                //Uri UriWebService = new Uri(String.Format(@"{0}" + "recu" + ".aspx?key={1}&rdlc={2}&module={3}", root, key, rdlcName, moduleName));
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
           catch (Exception ex )
           {
               string error = ex.Message;
               return string.Empty;
           }
          
       }

        /// <summary>
        /// Chaque client aura sa propre client qu'il enverra au service pour la prise 
        /// enc compte de l'impression
        /// NB L'adresse IP fera office de clé pour chaque client
        /// </summary>
        /// <returns></returns>
       public static string getKey()
       {
           try
           {
               string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
               int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf(@"clientbin");
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

    }
}
