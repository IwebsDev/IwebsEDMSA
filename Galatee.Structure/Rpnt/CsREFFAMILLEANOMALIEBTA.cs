using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsREFFAMILLEANOMALIEBTA
    {
		
		
[DataMember]
		public Int32 FAMILLEANOMALIE_ID { get; set; }
[DataMember]
		public String LIBELLE { get; set; }
[DataMember]
		public Boolean ESTFRAUDE { get; set; }
[DataMember]
public List<CsREFTYPEANOMALIEBTA> REFTYPEANOMALIEBTA { get; set; }

    }
}
