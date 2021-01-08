using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsREFTYPEANOMALIEBTA
    {
		
		
[DataMember]
		public Int32 TYPEANOMALIE_ID { get; set; }
[DataMember]
		public Int32 FAMILLEANOMALIE_ID { get; set; }
[DataMember]
		public String LIBELLE { get; set; }
[DataMember]
		public Int32? SIEGEANOMALIE_ID { get; set; }
[DataMember]
public CsREFFAMILLEANOMALIEBTA REFFAMILLEANOMALIEBTA { get; set; }
[DataMember]
public List<CsTBRESULTATCONTROLEBTA> TBRESULTATCONTROLEBTA { get; set; }
    }
}
