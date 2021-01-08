using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsNature
    {
       [DataMember] public string CODE { get; set; }
       [DataMember] public string LIBCOURT { get; set; }
       [DataMember] public string LIBELLE { get; set; }
       [DataMember] public string COMPTGENE { get; set; }
       [DataMember] public string DC { get; set; }
       [DataMember] public string CTRAIT { get; set; }
       [DataMember] public string TRANS { get; set; }
       [DataMember] public string COPER { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int PK_ID { get; set; }
    }
}
