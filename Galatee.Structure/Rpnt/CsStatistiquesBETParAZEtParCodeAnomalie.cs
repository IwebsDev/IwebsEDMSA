using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsStatistiquesBETParAZEtParCodeAnomalie
    {
		
		
[DataMember]
		public String CodeUO { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public Int32? Periodefact_An { get; set; }
[DataMember]
		public Int32? PeriodeFact_Mois { get; set; }
[DataMember]
		public Int32? NombreDeBETs { get; set; }
[DataMember]
		public Int32? NbreBETTraites { get; set; }
[DataMember]
		public String AZ_DisplayName { get; set; }
[DataMember]
		public String CodeAnomalie { get; set; }
[DataMember]
		public String LibelleAnamolie { get; set; }
[DataMember]
		public String MatriculeAgentZone { get; set; }


    }
}
