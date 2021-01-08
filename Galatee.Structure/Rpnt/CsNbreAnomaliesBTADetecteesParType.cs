using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsNbreAnomaliesBTADetecteesParType
    {
		
		
[DataMember]
		public String CodeUO { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String FamilleAnomalie_Libelle { get; set; }
[DataMember]
		public Int32 TypeAnomalie_ID { get; set; }
[DataMember]
		public String TypeAnomalie_Libelle { get; set; }
[DataMember]
		public Int32? Quantite { get; set; }


    }
}
