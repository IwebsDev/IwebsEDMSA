using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{


    [DataContract]
    public class CsDetailTarifAnnuel
    {

      [DataMember] public int PK_ID { get; set; }
      [DataMember] public Nullable<int> FK_IDTARIFANNUEL { get; set; }
      [DataMember] public Nullable<int> BARVOL { get; set; }
      [DataMember] public Nullable<decimal> BARPRIX { get; set; }
      [DataMember] public string TRANS { get; set; }
      [DataMember] public string USERCREATION { get; set; }
      [DataMember] public Nullable<System.DateTime> DATECREATION { get; set; }
    }

}
