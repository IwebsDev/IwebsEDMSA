using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsCampagneHTAAccessiblesParlUO
    {
		
		
[DataMember]
		public Guid Campagne_ID { get; set; }
[DataMember]
		public String Libelle_Campagne { get; set; }
[DataMember]
		public Int32 Statut_ID { get; set; }
[DataMember]
		public String MatriculeAgentCreation { get; set; }
[DataMember]
		public DateTime DateCreation { get; set; }
[DataMember]
		public Int32 NbreElements { get; set; }
[DataMember]
		public String MatriculeAgentDerniereModification { get; set; }
[DataMember]
		public DateTime? DateModification { get; set; }
[DataMember]
		public DateTime DateDebutControles { get; set; }
[DataMember]
		public DateTime DateFinPrevue { get; set; }
[DataMember]
		public String CodeUO { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }


    }
}
