using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
   public class CsLienRedevence
    {
        [DataMember]
        public int FK_IDREDEVANCE { get; set; }
        [DataMember]
        public int FK_IDREDEVANCEPARAMETRE { get; set; }
        [DataMember]
        public System.DateTime DATECREATION { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
        [DataMember]
        public virtual CsRedevance REDEVANCE { get; set; }
    }
}
