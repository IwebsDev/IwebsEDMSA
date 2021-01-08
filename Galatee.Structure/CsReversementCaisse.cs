using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsReversementCaisse : CsPrint
    {
      [DataMember] public int PK_ID { get; set; }
      [DataMember] public Nullable<System.DateTime> DATE { get; set; }
      [DataMember] public Nullable<decimal> MONTANT { get; set; }
      [DataMember] public Nullable<decimal> RESTE { get; set; }
      [DataMember] public int FK_IDHABILITATIONCAISSE { get; set; }
      [DataMember] public bool IsCAISSECOURANTE { get; set; }
                     

    }
}









