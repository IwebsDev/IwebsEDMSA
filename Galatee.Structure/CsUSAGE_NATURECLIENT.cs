using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsUSAGE_NATURECLIENT
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public Nullable<int> FK_IDUSAGE { get; set; }
        [DataMember]
        public Nullable<int> FK_IDNATURECLIENT { get; set; }
    }
}
