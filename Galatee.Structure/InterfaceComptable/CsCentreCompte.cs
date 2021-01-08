using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCentreCompte
    {
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public string CODEACTIVITE { get; set; }
       [DataMember] public string LIBELLEACTIVITE { get; set; }
       [DataMember] public string CODECENTRE { get; set; }
       [DataMember] public string CODECOMPTA { get; set; }
       [DataMember] public string DC { get; set; }
       [DataMember] public string CI { get; set; }
    }
}
