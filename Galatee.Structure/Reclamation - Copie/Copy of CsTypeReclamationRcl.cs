using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
   public class CsTypeReclamationRcl
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public Nullable<byte> Fk_IdGroupe { get; set; }
        [DataMember]
        public string Libelle { get; set; }
        [DataMember]
        public string Code { get; set; }
    }
}
