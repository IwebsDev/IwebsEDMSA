using Galatee.Silverlight.ServiceRecouvrement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Galatee.Silverlight.ServiceRpnt;


namespace Galatee.Silverlight.Rpnt.Helper
{
    public class BaseEventArgs : EventArgs
    {
        public object Data { get; set; }
    }
    public class BranchementClientEventArgs : BaseEventArgs
    {
        public List<CsClient> ListeClientEligibleSellection { get; set; }
        public object MethodeDetection { get; set; }
        public string PeriodeDepart { get; set; }
    }
    public class AnomalieEventArgs : BaseEventArgs
    {
        //public List<CsREFFAMILLEANOMALIEBTA> Famille { get; set; }
    }
    public class MethodeDetectionEventArgs : BaseEventArgs
    {
    }
    public class ParametreEventArgs : BaseEventArgs
    {
    }
}
