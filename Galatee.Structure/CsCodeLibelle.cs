using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
    public class CsCodeLibelle
    {
        [DataMember]
        public string Libelle { get; set; }
        [DataMember]
        public string Code { get; set; }
    }
}
