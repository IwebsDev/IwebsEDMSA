using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
   public class CsStatutJuridique
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public string CODE { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
    }
}
