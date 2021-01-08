using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBCONSTATHUISSIERHTA
    {
		
		
[DataMember]
		public Guid CONSTATFRAUDE_ID { get; set; }
[DataMember]
		public Guid HUISSIER_ID { get; set; }
[DataMember]
		public Boolean PAYE { get; set; }


    }
}
