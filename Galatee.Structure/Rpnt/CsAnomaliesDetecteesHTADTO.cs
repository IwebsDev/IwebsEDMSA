using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsAnomaliesDetecteesHTA
    {
		
		
[DataMember]
		public Guid ResultatControle_ID { get; set; }
[DataMember]
		public Int32 TypeAnomalie_ID { get; set; }
[DataMember]
		public String TypeAnomalie_Libelle { get; set; }
[DataMember]
		public Int32? FamilleAnomalie_ID { get; set; }
[DataMember]
		public String FamilleAnomalie_Libelle { get; set; }


    }
}
