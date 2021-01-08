using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsMapperLclientCoupure  
    {
        public int PK_ID { get; set; }
        public int FK_IDLCLIENT { get; set; }
        public string IDCOUPURE { get; set; }
    }
}
