using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBPARAMETRE
    {
		
		
[DataMember]
		public Int32 PARAMETRE_ID { get; set; }
[DataMember]
		public Int32 TYPEVALEUR { get; set; }
[DataMember]
		public String LIBELLEPARAMETRE { get; set; }
[DataMember]
		public String DESCRIPTION { get; set; }
[DataMember]
public bool? ISCYCLFAC { get; set; }
[DataMember]
		public String VALEUR { get; set; }



    }
}
