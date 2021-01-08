using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsEVOLUTIONBASE
    {
		
		
[DataMember]
		public String EVOLUTION_ID { get; set; }
[DataMember]
		public String VERSIONAPPLICATIVE { get; set; }
[DataMember]
		public DateTime? DATEEXECUTION { get; set; }


    }
}
