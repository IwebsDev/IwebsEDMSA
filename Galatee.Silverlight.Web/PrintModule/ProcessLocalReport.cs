using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using System.Drawing.Printing;
using System.IO;


namespace Galatee.Silverlight.Web
{
    public class ProcessLocalReport : System.Web.UI.Page
    {
        LocalReport localReport = new LocalReport();
        Page _ass;
        string fileLabel = string.Empty;
        public ProcessLocalReport(Page filename)
        {
             _ass = filename;
        }
        private void RenderReport<T>(List<T> Data)
        {
            try
            {
                if (Data.Count > 0)
                {
                    string bases = _ass.Request.Url.AbsoluteUri.Replace("//", "/");
                    int postion = bases.IndexOf(_ass.Request.Url.AbsoluteUri.Replace("//", "/").Split('/')[3]);
                    string dataset = "Reports/" + bases.Substring(postion).Split('.')[0] + ".rdlc";
                    string datasetName = dataset.Substring(dataset.LastIndexOf('/') + 1).Split('.')[0];
                    fileLabel = datasetName;
                    DataTable dt = new DataTable();
                    localReport.ReportPath = Server.MapPath("~/" + dataset);
                    ReportDataSource reportDataSource = new ReportDataSource(datasetName, Utility.ConvertToDataTable(Data));

                    localReport.DataSources.Add(reportDataSource);
                }
                else
                {
                    // There were no records returned
                    Response.Clear();
                    Response.Write("No Data");
                    Response.End();
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void RenderReport(DataTable Data)
        {
            try
            {
                if (Data.Rows.Count > 0 && Data !=null)
                {
                    string bases = _ass.Request.Url.AbsoluteUri.Replace("//", "/");
                    int postion = bases.IndexOf(_ass.Request.Url.AbsoluteUri.Replace("//", "/").Split('/')[3]);
                    string dataset = "Reports/" + bases.Substring(postion).Split('.')[0] + ".rdlc";
                    string datasetName = dataset.Substring(dataset.LastIndexOf('/')+1).Split('.')[0];
                    fileLabel = datasetName;
                    DataTable dt = new DataTable();
                    localReport.ReportPath = Server.MapPath("~/" + dataset);
                    ReportDataSource reportDataSource = new ReportDataSource(datasetName, Data);

                    localReport.DataSources.Add(reportDataSource);
                }
                else
                {
                    // There were no records returned
                    Response.Clear();
                    Response.Write("No Data");
                    Response.End();
                }
            }
            catch (Exception)
            {

                //throw;
            }

        }
        void CreatePDF(string filename,bool val)
        {
            try
            {
                PrinterSettings ps = new PrinterSettings();
                ImpressionDirect impression = new ImpressionDirect();
                string filePath = Server.MapPath("~")+@"PDF";
                impression.Run(localReport,val, filePath, filename, val);

            }
            catch (Exception)
            {

                //throw;
            }
            finally
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "close", "window.close()", true);
            }

        }

        void PrintPDF(string printername,bool orientation)
        {
            try
            {
                PrinterSettings ps = new PrinterSettings();
                string printer=string.Empty;
                if (!string.IsNullOrEmpty(printername))
                    printer = printername;
                else
                    printer = ps.PrinterName;
                ImpressionDirect impression = new ImpressionDirect();
                impression.Run(localReport,orientation, printer);

            }
            catch (Exception)
            {

                //throw;
            }
            finally
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "close", "window.close()", true);
            }

        }

        /// <summary>
        /// Trigger the creation of pdf file from localReport Content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data">Donnees de rapports</param>
        /// <param name="filename">Name of pdf file to create</param>
        public void Action<T>(List<T> Data,bool val)
        {
            RenderReport(Data);
            CreatePDF(fileLabel, val);
        }

        public void Action(DataTable Data, bool val)
        {
            RenderReport(Data);
            CreatePDF(fileLabel, val);
        }

        /// <summary>
        /// Trigger the printing from localReport Content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data">Donnees de rapports</param>
        /// <param name="filename">Name of the default of picked printer</param>
        public void Action<T>(List<T> Data, string printername)
        {
            RenderReport(Data);
            PrintPDF(printername,false);
        }

        public void Action(DataTable Data, string printername)
        {
            RenderReport(Data);
            PrintPDF(printername,false);
        }
    }
}