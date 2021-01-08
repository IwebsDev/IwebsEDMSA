using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Rpnt
{
    [DataContract]
    class CsTBCAMPAGNECONTROLEBTAPLUS
    {
        [DataMember]
        public int NBRLOTS { get; set; }
        [DataMember]
        public Int32 POULATIONNONAFFECTES { get; set; }

    }
}
