using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCATEGORIECLIENT_USAGE
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public Nullable<int> FK_IDCATEGORIECLIENT { get; set; }
        [DataMember]
        public Nullable<int> FK_IDUSAGE { get; set; }
    
    }
}
