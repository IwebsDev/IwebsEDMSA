using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
     public class CsTypeFraude
    {
          [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
         public string Code { get; set; }
        [DataMember]
        public string Libelle { get; set; }
        [DataMember]
        public int FK_IDORGANEFRAUDE { get; set; }

    }
}
