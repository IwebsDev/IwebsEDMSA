using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsREFDONNEESDEREFERENCE
    {
		
		
[DataMember]
		public Int32 DONNEEDEREFERENCE_ID { get; set; }
[DataMember]
		public String NAME { get; set; }
[DataMember]
		public String DESCRIPTION { get; set; }
[DataMember]
		public String TABLENAME { get; set; }
[DataMember]
		public Boolean? AUTORISERAJOUT { get; set; }
[DataMember]
		public Boolean? AUTORISERMODIFICATION { get; set; }


    }
}
