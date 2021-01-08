using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    [DataContract]
    public class CsUsage
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public string CODE { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
    }
}
