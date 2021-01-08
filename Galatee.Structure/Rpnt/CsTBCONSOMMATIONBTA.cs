using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBCONSOMMATIONBTA
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
		public String CODETYPECONSO { get; set; }
[DataMember]
		public Int32? PERIODEFACT_AN { get; set; }
[DataMember]
		public Int32? PERIODEFACT_MOIS { get; set; }
[DataMember]
		public String NUMFACT { get; set; }
[DataMember]
		public DateTime DATEFACT { get; set; }
[DataMember]
		public Int16 NBREDEJOURSDECONSO { get; set; }
[DataMember]
		public String NUMERO_COMPTEUR { get; set; }
[DataMember]
		public Int32? INDEXANC { get; set; }
[DataMember]
		public Int32? INDEXNVL { get; set; }


    }
}
