using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBCONTRATHTA
    {
		
		
[DataMember]
		public Guid CONTRAT_ID { get; set; }
[DataMember]
		public String REFERENCECLIENT { get; set; }
[DataMember]
		public String RACCORDEMENT_ID { get; set; }
[DataMember]
		public Guid? RESULTATDERNIERCONTROLE_ID { get; set; }
[DataMember]
		public Int32 STATUTCONTRAT { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public Double? PUISSANCESOUSCRITE { get; set; }
[DataMember]
		public String TYPETARIF_ID { get; set; }
[DataMember]
		public DateTime? DATEPROCHAINCONTROLEPOSTFRAUDE { get; set; }
[DataMember]
		public Int32? RAPPORTTC_INTENTREE { get; set; }
[DataMember]
		public Int32? RAPPORTTC_INTSORTIE { get; set; }
[DataMember]
		public Char? NIVEAUTENSION_ID { get; set; }
[DataMember]
		public Guid? CONSTATDUCONTROLEPOSTFRAUDE_ID { get; set; }
[DataMember]
		public DateTime? DATEDERNIERESELECTIONENCAMPAGNE { get; set; }


    }
}
