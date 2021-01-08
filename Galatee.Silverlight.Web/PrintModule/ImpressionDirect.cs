using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Drawing.Printing;
using System.Text;
using System.Drawing.Imaging;
using Galatee.Silverlight.Web.ServicePrintings;
using System.Management;
using System.ServiceModel;
using Galatee.Tools;

namespace Galatee.Silverlight.Web
{
    public class ImpressionDirect
    {
        List<System.IO.Stream> m_streams;
        private int m_currentPageIndex;

        private void Print(bool landscape, string NomDocument, string CheminImpression)
        {
            try
            {
                if (m_streams == null || m_streams.Count == 0)
                    return;

                PrintDocument printDoc = new PrintDocument();
                PrinterSettings ps = new PrinterSettings();
                ps.PrinterName = CheminImpression;
                ps.DefaultPageSettings.Landscape = landscape;
                ps.DefaultPageSettings.PrinterSettings.DefaultPageSettings.Landscape = landscape;
                printDoc.PrinterSettings = ps;

                if (!printDoc.PrinterSettings.IsValid)
                {
                    string msg = String.Format("Can't find printer \"{0}\".", ps.PrinterName);

                    PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                    printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), msg);
                    throw new Exception(msg);
                }
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                printDoc.Print();

                foreach (System.IO.Stream stream in m_streams)
                {
                    stream.Flush();
                    stream.Close();
                }

                // sending stream to local machine
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), ex.Message);
                throw ex;
            }

        } /// <summary>
        /// 
        private void PrintToLocalPrinter(string pPrinter,string port, string machine)
        {
            string GetPrintServiceAddress = string.Empty;
            try
            {
                if (m_streams == null || m_streams.Count == 0)
                    return;
                // sending stream to local machine
                MemoryStream ms = new MemoryStream();
                m_streams[0].CopyTo(ms);
                byte[] bytes = ms.ToArray();

                // CHK - 18/07/2013 - la methode "PrintReceipt" ne se trouve pas dans le service d'authentification
                // mais plutot dans celui de caisse. J'ai donc remplacé cette ligne par la suivante
                GetPrintServiceAddress = "http://" + machine + ":" + port + "/AuthentInitialize/AuthentInitializeService.svc"; 
                //GetPrintServiceAddress = "http://" + machine + ":" + port + "/Caisse/CaisseService.svc";
                InvokePrinter(GetPrintServiceAddress, bytes, pPrinter);
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), ex.Message + " url : " + GetPrintServiceAddress);
                throw ex;
            }

        } /// <summary>

        bool InvokePrinter(string ServiceUrl, byte[] bytes, string pPrinter)
        {
            string m  = string.Empty;
            try
            {
                string _service = string.Empty;
                ServiceInvoker invoker = new ServiceInvoker(new Uri(ServiceUrl));
                foreach (string service in invoker.AvailableServices)
                {
                    _service = service;
                    break;
                }
                List<string> methods = invoker.EnumerateServiceMethods(_service);

                foreach (string p in methods)
                    m += p + " || ";
                string method = "PrintReceipt";

                byte[] args = bytes;
                string[] argsPrinterName = new[] { pPrinter };
                bool? result = invoker.InvokeMethod<bool?>(_service, method, args);


                if ((result != null))
                {
                    if ((result.Value == true))
                        return true;
                    else
                    {
                        PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                        printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), m + " service : " + _service);
                        throw new Exception("Erreur dans l'appel du service d'impression");
                    
                    }
                }
                return true;
                //throw new Exception("Erreur dans l'appel du service d'impression");

            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), ex.Message);
                throw ex;
            }
        }

        private void Print(bool landscape, string NomDocument)
        {
            if (m_streams == null || m_streams.Count == 0)
                return;
            PrintDocument printDoc = new PrintDocument();
            PrinterSettings ps = new PrinterSettings();
            ps.DefaultPageSettings.Landscape = landscape;
            ps.DefaultPageSettings.PrinterSettings.DefaultPageSettings.Landscape = landscape;
            printDoc.DocumentName = NomDocument;
            printDoc.PrinterSettings = ps;


            if (!printDoc.PrinterSettings.IsValid)
            {
                string msg = String.Format("Can't find printer \"{0}\".", ps.PrinterName);
                // MessageBox.Show(msg, "Print Error");
                return;
            }
            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            printDoc.Print();

            foreach (System.IO.Stream stream in m_streams)
            {
                stream.Flush();
                stream.Close();
            }

        } /// <summary>



        private void Export(LocalReport report, float PageWidth, float PageHeight, float MarginTop, float MarginLeft, float MarginRight, float MarginBottom)
        {
            try
            {
                StringBuilder deviceInfosb = new StringBuilder();
                deviceInfosb.Append("<DeviceInfo>");
                deviceInfosb.Append("<OutputFormat>EMF</OutputFormat>");
                deviceInfosb.Append(string.Format("<PageWidth>{0}in</PageWidth>", PageWidth));
                deviceInfosb.Append(string.Format("<PageHeight>{0}in</PageHeight>", PageHeight));
                deviceInfosb.Append(string.Format("<MarginTop>{0}in</MarginTop>", MarginTop));
                deviceInfosb.Append(string.Format("<MarginLeft>{0}in</MarginLeft>", MarginLeft));
                deviceInfosb.Append(string.Format("<MarginRight>{0}in</MarginRight>", MarginRight));
                deviceInfosb.Append(string.Format("<MarginBottom>{0}in</MarginBottom>", MarginBottom));
                deviceInfosb.Append(string.Format("</DeviceInfo>"));
                string deviceInfo = deviceInfosb.ToString();
                Microsoft.Reporting.WebForms.Warning[] warnings;
                m_streams = new List<System.IO.Stream>();
                report.Render("Image", deviceInfo, CreateStream, out warnings);


                foreach (System.IO.Stream stream in m_streams)
                {
                    stream.Position = 0;
                }

                //PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
               // printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), "Export(LocalReport report, float PageWidth,");
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), ex.Message);
                throw ex;
            }

        }


        // Creation du fichier
        private void CreateFiles(LocalReport report, string CheminFichier, string NomDuFichier)
        {
            try
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), @CheminFichier);

                using (FileStream fs = new FileStream(@CheminFichier + "\\" + NomDuFichier + ".PDF", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    byte[] bytes = report.Render("PDF");
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                    fs.Close();
                }

            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), ex.Message);
                throw ex;
            }

        }

        // Export du fichier
        //private void CreateFiles(LocalReport report, string CheminFichier, string NomDuFichier,string Format)
        //{
        //    try
        //    {
        //        Warning[] warnings;
        //        string[] streamids;
        //        string mimeType;
        //        string encoding;
        //        string extension;

        //        PrintingsServiceClient printErrosr = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
        //        if(report!=null)
        //        printErrosr.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), "Format : " + Format + "| CheminFichier :" + CheminFichier + " | NomDuFichier" + NomDuFichier);
        //        else
        //            printErrosr.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), "Format : " + Format + "| CheminFichier :" + CheminFichier + " | NomDuFichier :" + NomDuFichier);

        //        if (Format == "xlsx")
        //        {
        //            byte[] bytes = report.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);
        //            FileStream fs = new FileStream(@CheminFichier + "\\" + NomDuFichier + ".xls", FileMode.Create);
        //            fs.Write(bytes, 0, bytes.Length);
        //            fs.Close();
        //        }
        //        if (Format == "doc")
        //        {
        //         byte[] bytes = report.Render("WORD", null, out mimeType, out encoding, out extension, out streamids, out warnings);

        //        FileStream fs = new FileStream(@CheminFichier + "\\" + NomDuFichier +".doc",FileMode.Create);
        //        fs.Write(bytes, 0, bytes.Length);
        //        fs.Close();
        //        }
        //        if (Format == "pdf")
        //        {
        //            byte[] bytes = report.Render("pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

        //            FileStream fs = new FileStream(@CheminFichier + "\\" + NomDuFichier + ".pdf", FileMode.Create);
        //            fs.Write(bytes, 0, bytes.Length);
        //            fs.Close();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
        //        printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(),"ICIFINAL" + ex.InnerException.Message );
        //        throw ex;
        //    }

        //}


        private void CreateFiles(LocalReport report, string CheminFichier, string NomDuFichier, string Format)
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string extension;
                PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), CheminFichier + "|" + NomDuFichier + "|" + Format);
                if (Format == "xlsx")
                {
                    byte[] bytes = report.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);
                    printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), "Générer byte");
                    FileStream fs = new FileStream(@CheminFichier + "\\" + NomDuFichier + ".xls", FileMode.Create);
                    printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), "Initialiser le filsStream");
                    fs.Write(bytes, 0, bytes.Length);
                    printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), "Création du fichier");

                    fs.Close();
                }
                if (Format == "doc")
                {
                    byte[] bytes = report.Render("WORD", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                    FileStream fs = new FileStream(@CheminFichier + "\\" + NomDuFichier + ".doc", FileMode.Create);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }
                if (Format == "pdf")
                {
                    byte[] bytes = report.Render("pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                    FileStream fs = new FileStream(@CheminFichier + "\\" + NomDuFichier + ".pdf", FileMode.Create);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }

            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), "ICIFINAL" + ex.InnerException.Message);
                throw ex;
            }

        }


        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            try
            {
                Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);
                ev.Graphics.DrawImage(pageImage, ev.PageBounds);
                m_currentPageIndex++;
                ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), ex.Message);
                throw ex;
            }
        }


        private System.IO.Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            try
            {
                //Stream stream = new FileStream(@"..\..\" + name +
                //   "." + "Pdf", FileMode.Create);
                //m_streams.Add(stream);

                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write((name + "." + "Pdf"));

                m_streams.Add(stream);
                return stream;
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), ex.Message);
                throw ex;
            }
        }
         
        public void Run(LocalReport report, bool orientation, string NomImprimante)
        {
            try
            {
                if (orientation)
                    Export(report, 11.69f, 8.26f, 0f, 0f, 0f, 0f);
                else
                    Export(report, 8.26f, 11.69f, 0f, 0f, 0f, 0f);
                m_currentPageIndex = 0;
                Print(orientation, NomImprimante);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RunToCashierPrinter(LocalReport report, string printer,string port, string machine)
        {
            try
            {
                bool orientation = true;
                if (orientation)
                    Export(report, 11.69f, 8.26f, 0f, 0f, 0f, 0f);
                else
                    Export(report, 8.26f, 11.69f, 0f, 0f, 0f, 0f);
                m_currentPageIndex = 0;
                PrintToLocalPrinter(printer,port, machine);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Run(LocalReport report, bool orientation, string CheminImpression, string NomFichier, bool ImpressionDsFichier)
        {
            try
            {
                if (!ImpressionDsFichier)
                {
                    if (orientation)
                        Export(report, 11.69f, 8.26f, 0f, 0f, 0f, 0f);
                    else
                        Export(report, 8.26f, 11.69f, 0f, 0f, 0f, 0f);
                    m_currentPageIndex = 0;
                    Print(orientation, NomFichier, CheminImpression);
                }
                else
                    CreateFiles(report, CheminImpression, NomFichier);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void RunCreationPDF(LocalReport report, bool orientation, string CheminImpression, string NomFichier, bool ImpressionDsFichier)
        {
            try
            {
                NomFichier = NomFichier + 
                    System.DateTime.Now.Day +  
                    System.DateTime.Now.Month +   
                    System.DateTime.Now.Year +  
                    System.DateTime.Now.Hour +  
                    System.DateTime.Now.Minute +
                    System.DateTime.Now.Second ;
               CreateFiles(report, CheminImpression, NomFichier);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RunExportFile(LocalReport report, bool orientation, string CheminImpression, string NomFichier, bool ImpressionDsFichier,string format)
        {
            try
            {
                NomFichier = NomFichier +
                    System.DateTime.Now.Day + 
                    System.DateTime.Now.Month + 
                    System.DateTime.Now.Year +  
                    System.DateTime.Now.Hour +  
                    System.DateTime.Now.Minute +
                    System.DateTime.Now.Second;
                CreateFiles(report, CheminImpression, NomFichier,format );
            }
            catch (Exception ex)
            {
                PrintingsServiceClient printErrosr = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
                printErrosr.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), "exception : " + ex.InnerException.Message );
                throw ex;
            }
        }

    }
}

