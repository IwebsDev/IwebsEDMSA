using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTrancheRedevence:CsPrint
    {
        [DataMember]
        public int FK_IDREDEVANCE { get; set; }
        [DataMember]
        public byte ORDRE { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public Nullable<bool> GRATUIT { get; set; }
        [DataMember]
        public  CsRedevance REDEVANCE { get; set; }
        [DataMember]
        public int PK_ID { get; set; }
    } 
}
