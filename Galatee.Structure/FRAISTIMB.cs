using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class FRAISTIMB : CsPrint
    {

        [DataMember]
        public decimal? BINF { get; set; }
        [DataMember]
        public decimal? BSUP { get; set; }
        [DataMember]
        public DateTime? DATEMAJ { get; set; }
        [DataMember]
        public DateTime? DMAJ { get; set; }
        [DataMember]
        public decimal? FRAIS { get; set; }
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public string TRANS { get; set; }
    }
}
