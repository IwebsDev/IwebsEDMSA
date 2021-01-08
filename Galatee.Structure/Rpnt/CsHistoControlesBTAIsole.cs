using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsHistoControlesBTAIsole
    {
		
		
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public Guid ResultatControle_ID { get; set; }
[DataMember]
		public Int32 ResultatValue { get; set; }


    }
}
