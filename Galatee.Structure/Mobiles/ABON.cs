using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    class ABON
    {
        [DataMember]
        public string REFCLIENT { get; set; }
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string TOURNEE { get; set; }
        [DataMember]
        public string NOMABON { get; set; }
        [DataMember]
        public string ORDRE { get; set; }
        [DataMember]
        public string ORDTOUR { get; set; }
    }
}
