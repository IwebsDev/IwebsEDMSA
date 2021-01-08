using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBPARAMETRAGESYNCHROAUTOMATIQUE
    {
		
		
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public DateTime? DATEDEBUTVALIDITE { get; set; }
[DataMember]
		public Boolean ESTACTIF { get; set; }
[DataMember]
		public Int32? PERIODICITE { get; set; }
[DataMember]
		public Int32? PARTIEHEURELANCEMENT_HEURE { get; set; }
[DataMember]
		public Int32? PARTIEHEURELANCEMENT_MINUTE { get; set; }
[DataMember]
		public DateTime? DATEPROCHAINEEXECUTION { get; set; }


    }
}
