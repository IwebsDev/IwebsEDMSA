using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
    [DataContract]
    public class CsREFLIGNEABAQUE_CHOIXTC
    {


        [DataMember]
        public String ABAQUE_ID { get; set; }
        [DataMember]
        public String PLAGE_RAPPORTTC { get; set; }
        [DataMember]
        public String PLAGE_PS { get; set; }
        [DataMember]
        public Int32 RAPPORTTC_INTENTREE { get; set; }
        [DataMember]
        public Int32 RAPPORTTC_INTSORTIE { get; set; }
        [DataMember]
        public Double PS_BORNEINF { get; set; }
        [DataMember]
        public Double PS_BORNESUP { get; set; }


    }
}
