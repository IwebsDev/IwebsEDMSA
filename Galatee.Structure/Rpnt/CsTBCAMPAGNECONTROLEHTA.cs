using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBCAMPAGNECONTROLEHTA
    {
		
		
[DataMember]
		public Guid CAMPAGNE_ID { get; set; }
[DataMember]
		public String LIBELLE_CAMPAGNE { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public Int32 STATUT_ID { get; set; }
[DataMember]
		public String MATRICULEAGENTCREATION { get; set; }
[DataMember]
		public DateTime DATECREATION { get; set; }
[DataMember]
		public Int32 NBREELEMENTS { get; set; }
[DataMember]
		public String MATRICULEAGENTDERNIEREMODIFICATION { get; set; }
[DataMember]
		public DateTime? DATEMODIFICATION { get; set; }
[DataMember]
		public DateTime DATEDEBUTCONTROLES { get; set; }
[DataMember]
		public DateTime DATEFINPREVUE { get; set; }


    }
}
