using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBHISTORIQUECRISOLE
    {
		
		
[DataMember]
		public Guid RESULTATCONTROLE_ID { get; set; }
[DataMember]
		public Guid CONTRAT_ID { get; set; }


    }
}
