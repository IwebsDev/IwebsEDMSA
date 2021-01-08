using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsSYNCHROENCOUR
    {
		
		
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public DateTime DATEDELANCEMENT { get; set; }
[DataMember]
		public String NOMMACHINE { get; set; }
[DataMember]
		public String MATRICULEUSER { get; set; }
[DataMember]
		public Boolean ESTSYNCHROMANUELLE { get; set; }
[DataMember]
		public Int32? ETAPESYNCHRO { get; set; }


    }
}
