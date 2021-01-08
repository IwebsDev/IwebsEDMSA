using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsStatistiquesDeTraitementDesBET
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


    }
}
