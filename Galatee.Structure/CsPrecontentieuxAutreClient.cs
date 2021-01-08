using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsPrecontentieuxAutreClient : CsPrint 
    {
      [DataMember] public Nullable<int> FK_IDCLIENTPRECONTENTIEUX { get; set; }
      [DataMember] public Nullable<int> FK_IDAUTRECLIENT { get; set; }
      [DataMember] public int PK_ID { get; set; }
    }
}









