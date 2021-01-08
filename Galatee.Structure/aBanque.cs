using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class aBanque : CsPrint
    {
        
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public string CODE { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
       [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
      
    }
}
