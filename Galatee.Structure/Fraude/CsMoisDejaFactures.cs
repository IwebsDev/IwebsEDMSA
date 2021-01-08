using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract] 
   public  class CsMoisDejaFactures
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public int OrdreMois { get; set; }
        [DataMember]
        public Nullable<int> ConsoDejaFacturee { get; set; }
        [DataMember]
        public int FK_IDCONSOMMATION { get; set; }
        [DataMember]
        public int FK_IDPRODUIT { get; set; }

    }
}
