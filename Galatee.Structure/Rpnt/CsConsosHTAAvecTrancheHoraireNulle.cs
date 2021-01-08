using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsConsosHTAAvecTrancheHoraireNulle
    {
		
		
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public String Raccordement_ID { get; set; }
[DataMember]
		public Int32 StatutContrat { get; set; }
[DataMember]
		public Int32? PeriodeFact_An { get; set; }
[DataMember]
		public Int32? PeriodeFact_Mois { get; set; }
[DataMember]
		public Decimal ValeurConso { get; set; }
[DataMember]
		public String CodeTypeConso { get; set; }
[DataMember]
		public Double ConsoActiveJour { get; set; }
[DataMember]
		public Double ConsoActiveNuit { get; set; }
[DataMember]
		public Double ConsoActivePointe { get; set; }


    }
}
