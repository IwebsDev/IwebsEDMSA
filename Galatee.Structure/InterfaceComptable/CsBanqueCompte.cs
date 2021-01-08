using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsBanqueCompte
    {
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public string CODECENTRE { get; set; }
       [DataMember] public string COMPTE { get; set; }
       [DataMember] public string BANQUE { get; set; }
    }
}
