using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CASIND
    {
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string CAS { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public string ENQUETE { get; set; }
        [DataMember]
        public string FACTURE { get; set; }
        [DataMember]
        public string LIBFAC { get; set; }
        [DataMember]
        public int CASGEN1 { get; set; }
    }
}
