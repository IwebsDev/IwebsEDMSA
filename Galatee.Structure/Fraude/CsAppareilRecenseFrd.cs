using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.Linq;

namespace Galatee.Structure
{
        [DataContract]
  public  class CsAppareilRecenseFrd
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public Nullable<int> NOMBRE { get; set; }
        [DataMember]
        public Nullable<int> PUISSANCEUNITAIRE { get; set; }
        [DataMember]
        public string OBSERVATION { get; set; }
        [DataMember]
        public int FK_IDCONTROLE { get; set; }
        [DataMember]
        public int FK_IDAPPAREIL { get; set; }
        [DataMember]
        public int FK_IDPRODUIT { get; set; }
        [DataMember]
        public int CODEAPPAREIL { get; set; } 
       
    
    }
}
