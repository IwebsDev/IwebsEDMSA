using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsHistoAnomaliesParClientsHTA
    {
		
		
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public String CodeUO { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public String Raccordement_ID { get; set; }
[DataMember]
		public Guid? ResultatDernierControle_ID { get; set; }
[DataMember]
		public Guid? Lot_ID { get; set; }
[DataMember]
		public Guid ResultatControle_ID { get; set; }
[DataMember]
		public Int32 TypeAnomalie_ID { get; set; }
[DataMember]
		public String TypeAnomalie_Libelle { get; set; }
[DataMember]
		public String FamilleAnomalie_Libelle { get; set; }
[DataMember]
		public DateTime DateControle { get; set; }


    }
}
