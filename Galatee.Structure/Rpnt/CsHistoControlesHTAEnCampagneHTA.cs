using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsHistoControlesHTAEnCampagneHTA
    {
		
		
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public Int32 Methode_ID { get; set; }
[DataMember]
		public Guid? ResultatControle_ID { get; set; }
[DataMember]
		public Int32? ResultatValue { get; set; }
[DataMember]
		public Double Difference { get; set; }


    }
}
