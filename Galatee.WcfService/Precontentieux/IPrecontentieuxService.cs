using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using Galatee.DataAccess;
using System.Data;


namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPrecontentieuxService" in both code and config file together.
    [ServiceContract]
    public interface IPrecontentieuxService
    {
        [OperationContract]
        void DoWork();
    }
}
