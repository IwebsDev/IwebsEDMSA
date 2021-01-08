using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTypeLienProduit : CsPrint
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public int FK_IDPRODUIT { get; set; }
        [DataMember]
        public int FK_IDTYPELIEN { get; set; }
    }
}
