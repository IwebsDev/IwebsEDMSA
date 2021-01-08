using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Management;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Galatee.Silverlight.Web
{
    public partial class index : System.Web.UI.Page
    {
        public string HostName { get; set; }
        public string UserIp { get; set; }
        public string DefaultPrinter { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //HostName = System.Net.Dns.GetHostEntry(Request.UserHostName).HostName;

            //System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(HostName);
            //System.Net.IPAddress[] addr = ipEntry.AddressList;
            //foreach (System.Net.IPAddress ip in addr)
            //{
            //    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //    {
            //        if (ip.ToString().StartsWith("192"))
            //        {
            //            UserIp = ip.ToString();
            //            break;
            //        }
            //    }
            //}
        }
    }
}