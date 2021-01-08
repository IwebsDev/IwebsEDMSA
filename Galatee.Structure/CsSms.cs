using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsSms
    {
        [DataMember]
        public Guid SMSID { get; set; }
        [DataMember]
        public string DESTINATEUR { get; set; }
        [DataMember]
        public string MESSAGE { get; set; }
        [DataMember]
        public DateTime? DATEEMISSION { get; set; }
        [DataMember]
        public int STATUTENVOI { get; set; }
        [DataMember]
        public int? NOMBREENVOI { get; set; }
        [DataMember]
        public DateTime DATEENREGISTREMENT { get; set; }
    }
}
