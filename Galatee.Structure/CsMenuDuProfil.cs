using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsMenuDuProfil : CsPrint
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public int FK_IDPROFIL { get; set; }
        [DataMember]
        public int FK_IDMENU { get; set; }
    }
}
