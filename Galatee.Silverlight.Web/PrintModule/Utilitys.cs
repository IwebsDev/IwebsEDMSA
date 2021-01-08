using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.Drawing.Printing;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using Galatee.Silverlight.Web.ServiceCaisse;
using Galatee.Silverlight.Web.ServiceAccueil;
using Galatee.Silverlight.Web.ServiceIndex;
using Galatee.Silverlight.Web.ServiceReport;
using Galatee.Silverlight.Web.ServiceWeb;
using System.ServiceModel;
using Galatee.Silverlight.Web.ServicePrintings;
using Galatee.Tools;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Galatee.Silverlight.Web
{
    public class Utilitys
    {
        Main p = null;
        LocalReport report = new LocalReport();
        public Dictionary<string, string> parameter = new Dictionary<string, string>();
        string keyPreview = string.Empty;
        static string AddresPrinter = string.Empty;
        static Dictionary<string, string> param = new Dictionary<string, string>();
        public Utilitys(Main _p)
        {
            p = _p;
        }
        public Utilitys()
        {
            p = new Main();
        }

        /// <summary>
        /// 
        /// Convert a list of object to a datatable 
        /// so that it could be pass as parameter to local
        /// report embedded sources
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varlist"></param>
        /// <returns></returns>
        /// 
        //DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        //{
        //    DataTable dtReturn = new DataTable();

        //    // column names 
        //    PropertyInfo[] oProps = null;

        //    if (varlist == null) return dtReturn;

        //    foreach (T rec in varlist)
        //    {
        //        // Use reflection to get property names, to create table, Only first time, others will follow 
        //        if (oProps == null)
        //        {
        //            oProps = ((Type)rec.GetType()).GetProperties();
        //            foreach (PropertyInfo pi in oProps)
        //            {
        //                Type colType = pi.PropertyType;

        //                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
        //                == typeof(Nullable<>)))
        //                {
        //                    colType = colType.GetGenericArguments()[0];
        //                }

        //                dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
        //            }
        //        }

        //        DataRow dr = dtReturn.NewRow();

        //        foreach (PropertyInfo pi in oProps)
        //        {
        //            dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
        //            (rec, null);
        //        }

        //        dtReturn.Rows.Add(dr);
        //    }
        //    return dtReturn;
        //}

        // CHK 20/02/2013
        /// <summary>
        /// Permet de lancer une impression a partir de la clé de l'utilisateur qui l'a lancée
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<object> getCollectionToPrint(string key)
        {
            try
            {

                EndpointAddress lePoint = EndPointPrinting(SessionObject.machine, SessionObject.portService);

                PrintingsServiceClient print = new PrintingsServiceClient(ProtocoleFacturation(), lePoint);
                List<Galatee.Silverlight.Web.ServicePrintings.CsPrint> connex = new List<Galatee.Silverlight.Web.ServicePrintings.CsPrint>();

                List<object> connexO = new List<object>();
                var resultatAImprimer = print.GetCsPrintFromWebPart(key);
                if (resultatAImprimer ==null )
                    print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Aucune valeur");

                connex.AddRange(resultatAImprimer);
                connexO.AddRange(connex);

                print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), key);

                if (connexO.Count == 0 )
                    print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Aucune données");
                else
                    print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "données trouve");

                parameter = print.getParameters(key);

                if (parameter!=null &&  parameter.Count != 0)
                    print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Parametre trouvé");
                else
                    print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Parametre non trouvé");

                return connexO;
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
                throw ex;
            }

        }
        public List<object> getCollectionToPrintAccueil(string key)
        {
            try
            {

                EndpointAddress lePoint = EndPointPrinting(SessionObject.machine, SessionObject.portService);

                PrintingsServiceClient print = new PrintingsServiceClient(ProtocoleFacturation(), lePoint);
                List<Galatee.Silverlight.Web.ServicePrintings.CsPrint> connex = new List<Galatee.Silverlight.Web.ServicePrintings.CsPrint>();

                List<object> connexO = new List<object>();
                var resultatAImprimer = print.GetCsPrintFromWebPartBalanceAgee(key);
                if (resultatAImprimer == null)
                    print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "BalanceAucune valeur");

                connex.AddRange(resultatAImprimer);
                connexO.AddRange(connex);

                print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), key);

                if (connexO != null && connexO.Count == 0)
                    print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "BalanceAucune données");
                else
                    print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Balance données trouve");

                parameter = print.getParameters(key);

                if (parameter != null &&  parameter.Count != 0)
                    print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Parametre trouvé");
                else
                    print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Parametre non trouvé");

                return connexO;
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
                throw ex;
            }

        }

        //public List<object> getCollectionToPrint_Image(string key)
        //{
        //    try
        //    {

        //        EndpointAddress lePoint = EndPointPrinting(SessionObject.machine, SessionObject.portService);

        //        Galatee.Silverlight.Web.ServiceAccueil.AcceuilServiceClient print = new Galatee.Silverlight.Web.ServiceAccueil.AcceuilServiceClient(ProtocoleFacturation(), lePoint);
        //        List<Galatee.Silverlight.Web.ServicePrintings.CsPrint> connex = new List<Galatee.Silverlight.Web.ServicePrintings.CsPrint>();

        //        List<object> connexO = new List<object>();
        //        var resultatAImprimer = print.GetCsPrintFromWebPart(key);
        //        if (resultatAImprimer ==null )
        //            print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Aucune valeur");

        //        connex.AddRange(resultatAImprimer);
        //        connexO.AddRange(connex);

        //        print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), key);

        //        if (connexO.Count == 0 )
        //            print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Aucune données");
        //        else
        //            print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "données trouve");

        //        parameter = print.getParameters(key);

        //        if (param.Count != 0)
        //            print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Parametre trouvé");
        //        else
        //            print.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Parametre non trouvé");

        //        return connexO;
        //    }
        //    catch (Exception ex)
        //    {
        //        PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());
        //        printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
        //        throw ex;
        //    }

        //}

        // Func delegate test CHK 15/02/2013
        /* CHK - 01/03/2013 - retrait du delegué func */
        //private DataTable LoadData(string key, Func<string, List<object>> GetDataFromModule)
        //{
        //    try
        //    {
        //        List<object> data = new List<object>();
        //        /data.AddRange(GetDataFromModule(key));
        //        List<object> liste = getCollectionToPrint(key);
        //        data.AddRange(liste);
        //        return Galatee.Tools.Utility.ListToDataTable(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /* CHK - 01/03/2013 - retrait du delegué func */
        private DataTable LoadData(string key,string moduleName)
        {
            try
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());

                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "loadData");
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), key);

                List<object> data = new List<object>();
                List<object> liste = getCollectionToPrint(key);
                data.AddRange(liste);

                if (data.Count == 0 )
                    printError.SetErrorsFromSilverlightWebPrinting("data", "data Aucun");
                else
                    printError.SetErrorsFromSilverlightWebPrinting("data", "data trouve");

                return Galatee.Tools.Utility.ListToDataTable(data);
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
                throw ex;
            }
        }
        private DataTable LoadData(string key, string moduleName,string TypeEdition)
        {
            try
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());

                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "loadData");
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), key);

                List<object> data = new List<object>();
                List<object> liste = new List<object>();
                if (TypeEdition == "1")
                    liste = getCollectionToPrintAccueil(key);
                 else if (TypeEdition == "2")
                    liste = getCollectionToPrintAccueil(key);
                else
                    liste = getCollectionToPrint(key);

                data.AddRange(liste);

                if (data.Count == 0)
                    printError.SetErrorsFromSilverlightWebPrinting("data", "data Aucun");
                else
                    printError.SetErrorsFromSilverlightWebPrinting("data", "data trouve data count: " + data.Count());

                return Galatee.Tools.Utility.ListToDataTable(data);
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// lance l'impresssion ou la creation du rendu du rapport
        /// seleon l'orientation est en portrait ou paysage
        /// </summary>
        /// <param name="filename">nom du fichier a créé ou a imprimé</param>
        /// <param name="orientation">détermine la position du rapport ( paysage ou portrait)</param>
        void PrintPDF(string filename, string printer, bool orientation)
        {
            try
            {
                PrinterSettings ps = new PrinterSettings();
                ImpressionDirect impression = new ImpressionDirect();
                // convertir les fichiers de rapport en format pdf 
                //impression.Run(report, orientation, p.Server.MapPath("~") + @"PDF", filename, true);
                if (string.IsNullOrEmpty(printer))
                    printer = ps.PrinterName;
                // imprimer les fichiers de rapport en fonction de l'orientation 
                impression.Run(report, orientation, printer);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void PrintPDF<T>(string filename, string printer, bool orientation,bool IsCreationOnly,List<T> ListeFacture) where T: new ()
        {
            try
            {
                PrinterSettings ps = new PrinterSettings();
                ImpressionDirect impression = new ImpressionDirect();


                // convertir les fichiers de rapport en format pdf 
                if (IsCreationOnly)
                {
                    impression.Run(report, orientation, p.Server.MapPath("~") + @"PDF", filename, true);
                }
                else
                {
                    if (string.IsNullOrEmpty(printer))
                        printer = ps.PrinterName;
                    // imprimer les fichiers de rapport en fonction de l'orientation 
                    impression.Run(report, orientation, printer);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void PrintToCashier(string printer,string port, string machine)
        {
            try
            {
                PrinterSettings ps = new PrinterSettings();
                ImpressionDirect impression = new ImpressionDirect();
                impression.RunToCashierPrinter(report, printer,port, machine);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        void PrintPDF(string filename, bool orientation)
        {

            try
            {
                PrinterSettings ps = new PrinterSettings();
                ImpressionDirect impression = new ImpressionDirect();
                impression.Run(report, orientation, p.Server.MapPath("~") + @"PDF", filename, true);

                impression.Run(report, orientation, p.Server.MapPath("~") + @"PDF", filename, true);
                impression.Run(report, orientation, ps.PrinterName);
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
                throw ex;
            }
        }

        public string RenderReportPreview(string rdlc, string moduleName, string key)
        {
            // CheckBox to see if there is any data
            try
            {
                DataTable Data = new DataTable();
                Data = LoadData(key,moduleName);

                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());

                if (Data != null )
                   printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(),"Donnee chargéé");

                if (Data.Rows.Count > 0)
                {

                    List<ReportParameter> listeParam = new List<ReportParameter>();

                    if (parameter != null && parameter.Count != 0)
                    {
                        foreach (KeyValuePair<string, string> pair in parameter)
                        {
                            ReportParameter param = new ReportParameter(pair.Key, pair.Value);
                            listeParam.Add(param);
                        }
                    }

                    report.ReportPath = p.Server.MapPath("~/Reports/" + moduleName + @"/" + rdlc + ".rdlc");
                    ReportDataSource reportDataSource = new ReportDataSource(rdlc, Data);

                    if (listeParam != null & listeParam.Count > 0)
                        report.SetParameters(listeParam);
                    report.DataSources.Add(reportDataSource);

                    // save report in static PreviewPrinting class
                    printError.SetErrorsFromSilverlightWebPrinting(p.Server.MapPath("~/Reports/" + moduleName + @"/" + rdlc + ".rdlc"), "Terminer");


                    Random random = new Random();
                    keyPreview = random.Next(2013, 3000).ToString() + "_" + random.Next(1986, 2013).ToString() + "_" + DateTime.Now.Second + "_" + DateTime.Now.Millisecond;
                    PrintingPrieview.reportDataSource.Add(keyPreview, reportDataSource);
                    PrintingPrieview.reportParameter.Add(keyPreview, listeParam);
                    PrintingPrieview.reportPath.Add(keyPreview, p.Server.MapPath("~/Reports/" + moduleName + @"/" + rdlc + ".rdlc"));
                    PrintingPrieview.datasourceName.Add(keyPreview, rdlc);

                    printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Terminer");
                    printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), keyPreview);

                }
                return keyPreview;

            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
                throw ex;
            }
        }

        //void RenderReport(string rdlc, string moduleName, string key, Func<string, List<object>> GetDataFromModule)
        //{
        //    // CheckBox to see if there is any data
        //    DataTable Data = new DataTable();
        //    Data = LoadData(key, GetDataFromModule);
        //    if (Data.Rows.Count > 0)
        //    {

        //        List<ReportParameter> listeParam = new List<ReportParameter>();

        //        if (parameter != null && parameter.Count != 0)
        //        {
        //            foreach (KeyValuePair<string, string> pair in parameter)
        //            {
        //                ReportParameter param = new ReportParameter(pair.Key, pair.Value);
        //                listeParam.Add(param);
        //            }
        //        }

        //        report.ReportPath = p.Server.MapPath("~/Reports/" + moduleName + @"/" + rdlc + ".rdlc");
        //        ReportDataSource reportDataSource = new ReportDataSource(rdlc, Data);

        //        if (listeParam != null & listeParam.Count > 0)
        //            report.SetParameters(listeParam);
        //        report.DataSources.Add(reportDataSource);
        //    }

        //}
       
        /* CHK - 01/03/2013 - retrait du delegué func pour faciliter le debogage */
        DataTable RenderReportMail(string rdlc, string moduleName, string key)
        {
            // CheckBox to see if there is any data
            try
            {
                DataTable Data = new DataTable();
                Data = LoadData(key, moduleName);
                return Data;
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
                throw ex;
            }

        }
        void RenderReport(string rdlc, string moduleName, string key)
        {
            // CheckBox to see if there is any data
            try
            {

                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());

                DataTable Data = new DataTable();
                Data = LoadData(key, moduleName);
                if (Data == null)
                    printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Collection vide");



                if (Data.Rows.Count > 0)
                {

                    List<ReportParameter> listeParam = new List<ReportParameter>();

                    if (parameter != null && parameter.Count != 0)
                    {
                        foreach (KeyValuePair<string, string> pair in parameter)
                        {
                            ReportParameter param = new ReportParameter(pair.Key, pair.Value);
                            listeParam.Add(param);
                        }
                    }

                    report.ReportPath = p.Server.MapPath("~/Reports/" + moduleName + @"/" + rdlc + ".rdlc");
                    printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod() + " | " + "~/Reports/" + moduleName + @"/" + rdlc + ".rdlc", "Chemin du fichier rdlc");

                    ReportDataSource reportDataSource = new ReportDataSource(rdlc, Data);

                    if (listeParam != null & listeParam.Count > 0)
                    {
                        report.SetParameters(listeParam);
                        printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod() + " | " + listeParam.First().Name + " : " + listeParam.First().Values + "  " + listeParam.Last().Name + " : " + listeParam.Last().Values, "Set Parameters");
                    }

                    report.DataSources.Add(reportDataSource);
                    printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Add DataSources ok");

                }
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
                throw ex;
            }

        }

        void RenderReport(string rdlc, string moduleName, string key,string TypeEdition)
        {
            // CheckBox to see if there is any data
            try
            {

                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());

                DataTable Data = new DataTable();
                Data = LoadData(key, moduleName, TypeEdition);

                if (Data.Rows.Count > 0)
                {
                    List<ReportParameter> listeParam = new List<ReportParameter>();
                    if (parameter != null && parameter.Count != 0)
                    {
                        foreach (KeyValuePair<string, string> pair in parameter)
                        {
                            ReportParameter param = new ReportParameter(pair.Key, pair.Value);
                            listeParam.Add(param);
                        }
                    }

                    report.ReportPath = p.Server.MapPath("~/Reports/" + moduleName + @"/" + rdlc + ".rdlc");
                    ReportDataSource reportDataSource = new ReportDataSource(rdlc, Data);
                    report.DataSources.Add(reportDataSource);

                    if (listeParam != null & listeParam.Count > 0)
                        report.SetParameters(listeParam);
                }
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.InnerException.Message);
                throw ex;
            }

        }

        void  ValoriserReport<T>(List<T> ListeAEditer, string modulename,string rdlc) where T : new()
        {
            // CheckBox to see if there is any data
            try
            {
                DataTable Data = new DataTable();
                Data = Galatee.Tools.Utility.ListToDataTable(ListeAEditer);
                if (Data.Rows.Count > 0)
                {

                    List<ReportParameter> listeParam = new List<ReportParameter>();

                    if (parameter != null && parameter.Count != 0)
                    {
                        foreach (KeyValuePair<string, string> pair in parameter)
                        {
                            ReportParameter param = new ReportParameter(pair.Key, pair.Value);
                            listeParam.Add(param);
                        }
                    }

                    report.ReportPath = p.Server.MapPath("~/Reports/" + modulename + @"/" + rdlc + ".rdlc");
                    ReportDataSource reportDataSource = new ReportDataSource(rdlc, Data);

                    if (report.DataSources != null)
                        report.DataSources.Clear();

                    if (listeParam != null & listeParam.Count > 0)
                        report.SetParameters(listeParam);
                    report.DataSources.Add(reportDataSource);
                }
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(), EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
                throw ex;
            }

        }

        /// <summary>
        /// Initialise le telechargement des données et lance 
        /// l'impression de données
        /// </summary>
        /// <param name="orientation">determine l'orientation portrait ou paysage</param>
        /// <param name="modulename">nom du module dans lequel se fera l'impression</param>
        /// <param name="printer">nom de l'imprimante choisie</param>
        /// <param name="rdlc">fichier de rapport dans le module </param>
        /// <param name="key">clé servant d'entrée pour le téléchargement des données coté service</param>
        /// <param name="GetDataFromModule">methode à invoquer pour le chargement des données</param>
        /// <returns></returns>
        //public bool LaunchPrinting(bool orientation, string modulename, string printer, string rdlc, string key, Func<string, List<object>> GetDataFromModule)
        //{
        //    try
        //    {

        //        RenderReport(rdlc, modulename, key, GetDataFromModule);
        //        PrintPDF(rdlc, printer, orientation);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //        return false;
        //    }
        //}

        /// <summary>
        /// Initialise le telechargement des données et lance 
        /// l'impression de données
        /// </summary>
        /// <param name="orientation">determine l'orientation portrait ou paysage</param>
        /// <param name="modulename">nom du module dans lequel se fera l'impression</param>
        /// <param name="printer">nom de l'imprimante choisie</param>
        /// <param name="rdlc">fichier de rapport dans le module </param>
        /// <param name="key">clé servant d'entrée pour le téléchargement des données coté service</param>
        /// <param name="GetDataFromModule">methode à invoquer pour le chargement des données</param>
        /// <returns></returns>
        public string LaunchPrintingPreview(bool orientation, string rdlc, string modulename, string key)
        {
            try
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), "LaunchPrintingPreview");

                return RenderReportPreview(rdlc, modulename, key);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return string.Empty;
            }
        }

        /* CHK - 01/03/2013 - retrait du delegué func pour faciliter le debogage */
        public bool LaunchPrinting(bool orientation, string modulename, string printer, string rdlc, string key)
        {
            try
            {
                RenderReport(rdlc, modulename, key);
                PrintPDF(rdlc, printer, orientation);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SendMail(string address)
        {
            string MonAdresse = "iwebsmail@edm-sa.ml.com";
            MailAddress from = new MailAddress(MonAdresse);
            MailAddress to = new MailAddress(MonAdresse);
            MailMessage mailMsg = new MailMessage(from, to);

            mailMsg.Subject = "Votre facture d'electricité";
            mailMsg.Body = "Bonjour, vous trouverez ci-joint votre facture, merci de ne plus fraudez et de payer regulièrement \nAu plaisir.";

            SmtpClient client = new SmtpClient("10.100.2.54");
            client.Send(mailMsg);
        }


        private void SendMail(string address, string filePath)
        {
            string MonAdresse = "GalateeTeam@inova.ci";
            MailAddress from = new MailAddress(MonAdresse);
            MailAddress to = new MailAddress(address);
            MailMessage mailMsg = new MailMessage(from, to);

            mailMsg.Subject = "Votre facture d'electricité";
            mailMsg.Attachments.Add(new Attachment(filePath));
            mailMsg.Body = "Bonjour, vous trouverez ci-joint votre facture, merci de ne plus fraudez et de payer regulièrement \nAu plaisir.";

            SmtpClient client = new SmtpClient("iv-srv-003");
            client.Send(mailMsg);
        }

        public bool? LaunchPrinting(bool orientation, string modulename, string printer, string rdlc, string key, bool IsCreationOnly)
        {
            try
            {
                DataTable dt =  RenderReportMail(rdlc, modulename, key);
                List<ServicePrintings.CsFactureClient> ListeFacture = Galatee.Tools.Utility.GetEntityListFromQuery<ServicePrintings.CsFactureClient>(dt);
                List<ServicePrintings.CsFactureClient> tempon = new List<ServicePrintings.CsFactureClient>();
                //string rootFile  = p.Server.MapPath("~") + @"PDF";
                string cheminFacture = p.Server.MapPath("~") + @"PDF\\" + rdlc + ".PDF";
                Parallel.ForEach(ListeFacture, m => Launch(orientation, modulename, printer, rdlc, IsCreationOnly, ListeFacture, tempon, cheminFacture, m));               
                return true;               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Launch(bool orientation, string modulename, string printer, string rdlc, bool IsCreationOnly, List<ServicePrintings.CsFactureClient> ListeFacture, List<ServicePrintings.CsFactureClient> tempon, string cheminFacture, ServicePrintings.CsFactureClient m)
        {
            if (tempon.FirstOrDefault(f => f.Centre == m.Centre && f.Client == m.Client && f.Ordre == m.Ordre) == null)
            {
                List<ServicePrintings.CsFactureClient> factureAEditer = ListeFacture.Where(f => f.Centre == m.Centre && f.Client == m.Client && f.Ordre == m.Ordre).ToList();
                ValoriserReport(factureAEditer, modulename, rdlc);
                PrintPDF(rdlc, printer, orientation, IsCreationOnly, factureAEditer);
               //SendMail(m.Email, cheminFacture);

                tempon.Add(m);
            }
        }
        public bool LaunchCashPrint(string printer, string modulename, string rdlc, string key, string port, string machine)
        {
            try
            {
                RenderReport(rdlc, modulename, key);
                PrintToCashier(printer,port, machine);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Creation d'un fichier pdf a partir du rdlc */

        // 11/03/2012 par CHK
        public string LaunchPDFgeneration(bool orientation, string modulename, string rdlc, string key,Dictionary<string,string> param=null)
        {
            try
            {
                param = new Dictionary<string, string>();
                RenderReport(rdlc, modulename, key);
                return CreatePDF(rdlc, null, orientation);
                
                //return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw ex;
                //return null;
            }
        }

        string CreatePDF(string filename, string printer, bool orientation)
        {

            PrinterSettings ps = new PrinterSettings();
            ImpressionDirect impression = new ImpressionDirect();
            // convertir les fichiers de rapport en format pdf 
            string cheminFacture = p.Server.MapPath("~") + @"PDF\\" + filename + ".PDF";
            impression.Run(report, orientation, p.Server.MapPath("~") + @"PDF", filename, true);

            // On retourne le chemin du fichier pdf
            return cheminFacture;
        }

        public string CreatePDFChemin(string filename, string cheminFacture, bool orientation, string rdlc, string modulename, string key)
        {

            param = new Dictionary<string, string>();
            RenderReport(rdlc, modulename, key);
            PrinterSettings ps = new PrinterSettings();
            ImpressionDirect impression = new ImpressionDirect();
            impression.RunCreationPDF(report, orientation, cheminFacture, filename, true);

            // On retourne le chemin du fichier pdf
            return cheminFacture;
        }
        public string ExportChemin(string filename, string cheminFacture, bool orientation, string rdlc, string modulename, string key, string format)
        {
            PrintingsServiceClient printErrosr = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());

            param = new Dictionary<string, string>();
            RenderReport(rdlc, modulename, key);
            PrinterSettings ps = new PrinterSettings();
            ImpressionDirect impression = new ImpressionDirect();
            printErrosr.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Debut RunExportFile");
            impression.RunExportFile(report, orientation, cheminFacture, filename, true, format);
            printErrosr.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Fin RunExportFile");

            // On retourne le chemin du fichier pdf
            return cheminFacture;
        }
        public string ExportChemin(string filename, string cheminFacture, bool orientation, string rdlc, string modulename, string key, string format, string TypeEdition)
        {
            PrintingsServiceClient printErrosr = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());

            param = new Dictionary<string, string>();
            RenderReport(rdlc, modulename, key,TypeEdition );
            PrinterSettings ps = new PrinterSettings();
            ImpressionDirect impression = new ImpressionDirect();
            printErrosr.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), "RenderReport retour ok");

            impression.RunExportFile(report, orientation, cheminFacture, filename, true, format);

            printErrosr.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), "ICI3");

            // On retourne le chemin du fichier pdf
            return cheminFacture;
        }
        #endregion

        public EndpointAddress EndPoint()
        {
            try
            {
                return new EndpointAddress(GetPrintServiceAddress());
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        public BasicHttpBinding Protocole()
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
                throw ex;
            }
        }

        /// <summary>
        /// Crée un Binding pour la liaison WCF du service facturation
        /// </summary>
        /// <returns>Un objet de liaison</returns>
        public BasicHttpBinding ProtocoleFacturation()
        {
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                binding.OpenTimeout = new TimeSpan(0, 30, 0);
                binding.ReceiveTimeout  = new TimeSpan(0, 10, 0);
                binding.CloseTimeout = new TimeSpan(0, 10, 0);
                binding.SendTimeout = new TimeSpan(2, 0, 0);

                binding.MaxReceivedMessageSize = Int32.MaxValue;
                binding.MaxBufferSize = Int32.MaxValue;

                binding.ReaderQuotas.MaxDepth =32;
                binding.ReaderQuotas.MaxStringContentLength  =8192;
                binding.ReaderQuotas.MaxArrayLength   =Int32.MaxValue;
                binding.ReaderQuotas.MaxBytesPerRead    =Int32.MaxValue;
                binding.ReaderQuotas.MaxNameTableCharCount     =Int32.MaxValue;
                return binding;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetPrintServiceAddress()
        {
            try
            {
                if (string.IsNullOrEmpty(AddresPrinter))
                {
                    string caissePath = HttpContext.Current.Server.MapPath(@"~/Config/ClientConfig.xml");
                    //String caissePath = System.Web.HttpServerUtility.MapPath();
                    String contenuXml = String.Empty;

                    using (FileStream stream = new FileStream(caissePath, FileMode.Open,FileAccess.Read))
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

                            ConfigClass config = listeconfigs.FirstOrDefault(print => print.module.Equals("Printing"));
                            AddresPrinter = config.address;
                            return AddresPrinter;
                        }
                    }
                }
                else
                    return AddresPrinter;
            }
            catch (Exception ex)
            {
                //PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(),EndPoint());
                //printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
                throw ex;
            }
        }

        public EndpointAddress EndPointPrinting(string machine, string port)
        {
            try
            {
                string address = "http://" + machine + ":" + port + "/Printings/PrintingsService.svc";
                return new EndpointAddress(address);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        //public string GetPrintServiceAddress()
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(AddresPrinter))
        //        {
        //            string caissePath = HttpContext.Current.Server.MapPath(@"~/Config/ClientConfig.xml");
        //            //String caissePath = System.Web.HttpServerUtility.MapPath();
        //            String contenuXml = String.Empty;

        //            using (FileStream stream = new FileStream(caissePath, FileMode.Open, FileAccess.Read))
        //            {
        //                using (StreamReader reader = new StreamReader(stream))
        //                {
        //                    List<ConfigClass> listeconfigs = new List<ConfigClass>();
        //                    XmlSerializer serializer = new XmlSerializer(typeof(ConfigClass));
        //                    contenuXml = reader.ReadToEnd();

        //                    XElement rootElement = XElement.Parse(contenuXml);
        //                    IEnumerable<XElement> caisses = rootElement.Descendants("ConfigClass");

        //                    foreach (XElement el in caisses)
        //                        using (MemoryStream memStream = new MemoryStream(Encoding.Unicode.GetBytes(el.ToString())))
        //                        {
        //                            listeconfigs.Add((ConfigClass)serializer.Deserialize(memStream));
        //                        }

        //                    ConfigClass config = listeconfigs.FirstOrDefault(print => print.module.Equals("Printing"));
        //                    AddresPrinter = config.address;
        //                    return AddresPrinter;
        //                }
        //            }
        //        }
        //        else
        //            return AddresPrinter;
        //    }
        //    catch (Exception ex)
        //    {
        //        //PrintingsServiceClient printError = new PrintingsServiceClient(Protocole(),EndPoint());
        //        //printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
        //        throw ex;
        //    }
        //}

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(2);

            return sf.GetMethod().Name;
        }
    }
}


