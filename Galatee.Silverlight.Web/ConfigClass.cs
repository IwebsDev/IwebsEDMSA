using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Galatee.Silverlight.Web
{
    public class ConfigClass
    {
        public string module { get; set; }
        public string address { get; set; }
        public string binding { get; set; }
        public string bindingConfiguration { get; set; }
        public string contract { get; set; }
        public string maxBufferSize { get; set; }
        public string maxReceivedMessageSize { get; set; }
        public string security { get; set; }
    }
}