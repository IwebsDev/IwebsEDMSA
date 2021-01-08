using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CustumEclairagePublic:CsPrint
    {
        [DataMember]
        public string CODECENTRE { get; set; }
        [DataMember]
        public string LIBELLECENTRE { get; set; }
        [DataMember]
        public int NOMBRE { get; set; }
        [DataMember]
        public decimal? MONTANT { get; set; }
        [DataMember]
        public string PERIODE { get; set; }
        [DataMember]
        public bool SELECTIONNE { get; set; }
    }
}
