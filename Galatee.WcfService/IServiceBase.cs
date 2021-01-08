using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace WcfService
{
       [ServiceContract]
    public interface IServiceBase
    {
         [OperationContract]
         List<object> GetDataFromWebPart(string key);
         [OperationContract]
         bool? DeleteDataFromWebPart(string key);
    }
}
