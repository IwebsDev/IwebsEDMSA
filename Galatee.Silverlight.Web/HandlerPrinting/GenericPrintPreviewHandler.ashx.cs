using Galatee.Silverlight.Web.ServicePrintings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Galatee.Silverlight.Web.HandlerPrinting
{
    /// <summary>
    /// Summary description for GenericPrintPreviewHandler
    /// </summary>
    public class GenericPrintPreviewHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int bytelength = context.Request.ContentLength;
            byte[] inputbytes = context.Request.BinaryRead(bytelength);
            string message = System.Text.Encoding.UTF8.GetString(inputbytes);
 

           

            string[] parameters = message.Split('|');

            SessionObject.machine = parameters[3];
            SessionObject.port = parameters[4];
            SessionObject.portService = parameters[4];

            PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
            printError.SetErrorsFromSilverlightWebPrinting(new Utilitys().GetCurrentMethod(), "ProcessRequest");

            try
            {
                Utilitys objetUtil = new Utilitys();
                string keypreview= objetUtil.LaunchPrintingPreview(false, parameters[0], parameters[1], parameters[2]);
                context.Response.Write(keypreview);
            }
            catch (Exception)
            {
                context.Response.Write(string.Empty);
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