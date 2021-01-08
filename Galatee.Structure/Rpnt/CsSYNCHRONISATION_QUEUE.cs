using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsSYNCHRONISATION_QUEUE
    {
		
		
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public String MATRICULEUSER { get; set; }
[DataMember]
		public String NOMMACHINE { get; set; }
[DataMember]
		public Boolean ESTSYNCHROMANULLE { get; set; }
[DataMember]
		public DateTime DATEINITIALISATIONDEMANDE { get; set; }
[DataMember]
		public Int32 ETAPESYNCHRO { get; set; }
[DataMember]
		public Boolean ARRETDEMANDE { get; set; }
[DataMember]
		public Guid? IDSYNCHRONISATION { get; set; }


    }
}
