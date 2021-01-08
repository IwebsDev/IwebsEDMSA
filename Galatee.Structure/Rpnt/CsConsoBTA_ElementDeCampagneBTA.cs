using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsConsoBTA_ElementDeCampagneBTA
    {
		
		
[DataMember]
		public Guid Consommation_ID { get; set; }
[DataMember]
		public Decimal ValeurConso { get; set; }
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public Int32? Periodefact_An { get; set; }
[DataMember]
		public Int32? PeriodeFact_Mois { get; set; }
[DataMember]
		public DateTime DateFact { get; set; }
[DataMember]
		public String CodeTypeConso { get; set; }
[DataMember]
		public Guid Campagne_ID { get; set; }


    }
}
