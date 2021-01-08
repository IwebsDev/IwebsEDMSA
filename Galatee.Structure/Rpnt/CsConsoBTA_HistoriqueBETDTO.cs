using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsConsoBTA_HistoriqueBET
    {
		
		
[DataMember]
		public Decimal ValeurConso { get; set; }
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String CodeTypeConso { get; set; }
[DataMember]
		public Int32? Periodefact_An { get; set; }
[DataMember]
		public Int32? PeriodeFact_Mois { get; set; }
[DataMember]
		public Int16 NbreDeJoursDeConso { get; set; }
[DataMember]
		public Int32? IndexAnc { get; set; }
[DataMember]
		public Int32? IndexNvl { get; set; }
[DataMember]
		public Guid Consommation_ID { get; set; }
[DataMember]
		public String Numero_Compteur { get; set; }
[DataMember]
		public DateTime? DateFact { get; set; }


    }
}
