using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBCONSOMMATIONHTA
    {
		
		
[DataMember]
		public Guid CONSOMMATION_ID { get; set; }
[DataMember]
		public Decimal VALEURCONSO { get; set; }
[DataMember]
		public Guid CONTRAT_ID { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public Int32? PERIODEFACT_AN { get; set; }
[DataMember]
		public Int32? PERIODEFACT_MOIS { get; set; }
[DataMember]
		public DateTime DATEFACT { get; set; }
[DataMember]
		public String CODETYPECONSO { get; set; }
[DataMember]
		public String NUMFACT { get; set; }
[DataMember]
		public Double? PUISSANCEATTEINTE { get; set; }
[DataMember]
		public Double? TANGENTEPHI { get; set; }
[DataMember]
		public Double? CONSOACTIVEJ { get; set; }
[DataMember]
		public Double? CONSOACTIVEN { get; set; }
[DataMember]
		public Double? CONSOACTIVEP { get; set; }


    }
}
