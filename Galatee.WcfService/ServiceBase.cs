using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WcfService
{
     [DataContract]
     //[KnownType(typeof(ReportService))]
    public class ServiceBase : IServiceBase
    {
         public virtual List<object> GetDataFromWebPart(string key) { return null;}
         public virtual bool? DeleteDataFromWebPart(string key) { return null; }
    }
}