using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsAGR
    {
        [DataMember] public long  AGR_IG { get; set; }
        [DataMember] public long  LOCATION_TYPE_ID { get; set; }
        [DataMember] public long  LOCATION_STATUS_ID { get; set; }
        [DataMember] public string   ADDR1 { get; set; }
        [DataMember] public string   ADDR2 { get; set; }
        [DataMember] public string   ADDR3 { get; set; }
        [DataMember] public string   LOCATION_REF { get; set; }
        [DataMember] public long   AREA_ID { get; set; }
        [DataMember] public long   OWNER_LEGAL_ENTITY_ID { get; set; }
        [DataMember] public long   RESP_OPERATOR_ID { get; set; }

        
    }
}
