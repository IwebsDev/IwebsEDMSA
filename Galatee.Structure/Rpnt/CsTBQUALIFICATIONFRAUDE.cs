using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBQUALIFICATIONFRAUDE
    {
		
		
[DataMember]
		public Guid CONSTATFRAUDE_ID { get; set; }
[DataMember]
		public Int32 TYPEANOMALIE_ID { get; set; }


    }
}
