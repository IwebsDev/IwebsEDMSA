using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Rpnt
{
    [DataContract]
    public class CsRefMethodesDeDetectionClientsBTA
    {
        [DataMember]
        public int Methode_ID { get; set; }
        [DataMember]
        public string Libele_Methode { get; set; }
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public List<CstbParametres> ListParametres { get; set; }
    }
}
