using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsConsosHTAAvecPANonConformeATC
    {
		
		
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public Int32? PeriodeFact_An { get; set; }
[DataMember]
		public Int32? PeriodeFact_Mois { get; set; }
[DataMember]
		public String ABAQUE_ID { get; set; }
[DataMember]
		public Double PS_BorneInf { get; set; }
[DataMember]
		public Double PS_BorneSup { get; set; }
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public String Raccordement_ID { get; set; }
[DataMember]
		public Int32 StatutContrat { get; set; }
[DataMember]
		public Char? NiveauTENSION_ID { get; set; }
[DataMember]
		public Int32? TypeComptageHTA { get; set; }
[DataMember]
		public Int32? RapportTC_INTENTREE { get; set; }
[DataMember]
		public Int32? RapportTC_INTSORTIE { get; set; }
[DataMember]
		public Decimal ValeurConso { get; set; }
[DataMember]
		public String CodeTypeConso { get; set; }
[DataMember]
		public Guid Consommation_ID { get; set; }
[DataMember]
		public Double? PuissanceSouscrite { get; set; }
[DataMember]
		public Double? PuissanceAtteinte { get; set; }


    }
}
