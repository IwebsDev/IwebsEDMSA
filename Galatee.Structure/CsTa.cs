using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{


    [DataContract]
    public class CsTa:CsPrint
    {
        [DataMember]
        public Int32  NUM { get; set; }
        [DataMember]
        public string TRANS { get; set; }
        [DataMember]
        public DateTime ? DMAJ { get; set; }
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public string CODE { get; set; }
        [DataMember]
        public string DESCRIPTION { get; set; }
        [DataMember]
        public bool COCHER { get; set; }
    }

}
