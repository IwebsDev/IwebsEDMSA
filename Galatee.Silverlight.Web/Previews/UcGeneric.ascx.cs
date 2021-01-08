using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Galatee.Silverlight.Web.ServicePrintings;
using System.Data;

namespace Galatee.Silverlight.Web.Previews
{
    public partial class UcGeneric : System.Web.UI.UserControl
    {
        public string ServiceAddress = string.Empty;
        public string tModule = string.Empty;
        public string Rdlc = string.Empty;
        public string Matricule = string.Empty;
        public string DefinedRDLC = string.Empty;
        public Dictionary<string, string> Parameters = new Dictionary<string,string> ();

        public virtual DataTable GetPrintObjects(string module)
        {
            return null;
        }
    }
}