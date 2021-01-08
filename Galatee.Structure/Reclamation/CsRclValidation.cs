using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsRclValidation
    {
        [DataMember]
        public byte  PK_ID { get; set; }
        [DataMember]
        public string Libelle { get; set; }
        [DataMember]
        public string Code { get; set; }
    }
}
