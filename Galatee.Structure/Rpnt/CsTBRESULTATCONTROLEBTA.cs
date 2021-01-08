using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBRESULTATCONTROLEBTA
    {
		
		
[DataMember]
		public Guid RESULTATCONTROLE_ID { get; set; }
[DataMember]
		public Int32 RESULTATVALUE { get; set; }
[DataMember]
		public String MATRICULEAGENTSAISIE { get; set; }
[DataMember]
		public String MATRICULEAGENTCONTROLE { get; set; }
[DataMember]
		public DateTime DATESAISIE { get; set; }
[DataMember]
		public DateTime DATECONTROLE { get; set; }
[DataMember]
		public Int32? TYPEDECONTROLE { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public String COMMENTAIRES { get; set; }
[DataMember]
		public Guid? CONSTATFRAUDE_ID { get; set; }
[DataMember]
		public Guid? ID_FICHEVERIFICATIONSCANNEE { get; set; }


    }
}
