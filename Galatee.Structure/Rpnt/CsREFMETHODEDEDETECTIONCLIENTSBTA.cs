using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Galatee.Structure.Rpnt;


namespace  Galatee.Structure

{
    [DataContract]
    public class CsREFMETHODEDEDETECTIONCLIENTSBTA
    {
        [DataMember]
        public Int32? METHODE_ID { get; set; }
        [DataMember]
        public String LIBELE_METHODE { get; set; }
        [DataMember]
        public String METHODE { get; set; }
        [DataMember]
        public String DESCRIPTION { get; set; }
        [DataMember]
        public List<CsTBPARAMETRE> PARAMETTRES { get; set; }
        [DataMember]
        public List<CsTBMethodeDedectectionClientsBTA_PARAMETRE> tag { get; set; }

    }
}
