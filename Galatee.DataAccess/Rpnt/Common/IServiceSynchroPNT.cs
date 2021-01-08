using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galatee.DataAccess.Common
{
    public interface IServiceSynchroPNT
    {
        bool EstServicePRESynchronisation();
        bool TryConnectWCFEndPoint(string pEndPointAddress, string pConfigPath, ref string pErrMsg);
    }
}
