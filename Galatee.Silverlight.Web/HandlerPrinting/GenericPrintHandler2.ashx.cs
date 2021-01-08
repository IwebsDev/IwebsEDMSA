using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Galatee.Silverlight.Web.HandlerPrinting
{
    /// <summary>
    /// Description résumée de GenericPrintHandler
    /// </summary>
    public class GenericPrintHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int bytelength = context.Request.ContentLength;
            byte[] inputbytes = context.Request.BinaryRead(bytelength);
            string message = System.Text.Encoding.UTF8.GetString(inputbytes);

            string[] parameters = message.Split('|');

            try
            {
                Utilitys objetUtil = new Utilitys();
                //objetUtil.LaunchPrinting(false, parameters[1], parameters[3], parameters[0], parameters[2], objetUtil.getCollectionToPrint);
            }
            catch (Exception)
            {
                throw;
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