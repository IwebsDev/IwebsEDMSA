using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsListeAnomaliesHTADetectee
    {
		
		
[DataMember]
		public String CodeUO { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public DateTime DateSaisie { get; set; }
[DataMember]
		public DateTime DateControle { get; set; }
[DataMember]
		public Int32 TypeAnomalie_ID { get; set; }
[DataMember]
		public String TypeAnomalie_Libelle { get; set; }
[DataMember]
		public String FamilleAnomalie_Libelle { get; set; }
[DataMember]
		public Int32 FamilleAnomalie_ID { get; set; }


    }
}
