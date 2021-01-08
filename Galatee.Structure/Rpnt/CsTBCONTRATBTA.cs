using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBCONTRATBTA
    {
		
		
[DataMember]
		public Guid CONTRAT_ID { get; set; }
[DataMember]
		public String REFERENCECLIENT { get; set; }
[DataMember]
		public String BRANCHEMENT_ID { get; set; }
[DataMember]
		public Guid? RESULTATDERNIERCONTROLE_ID { get; set; }
[DataMember]
		public Int32 STATUTCONTRAT { get; set; }
[DataMember]
		public Int32? GROUPEDEFACTURATION { get; set; }
[DataMember]
		public Double? PUISSANCESOUSCRITE { get; set; }
[DataMember]
		public Decimal? CONSOMOYENNE { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public Int32? TYPECONTRATID { get; set; }
[DataMember]
		public String TYPETARIF_ID { get; set; }
[DataMember]
		public Char? USAGEABONNEMENT_ID { get; set; }
[DataMember]
		public DateTime? DATEABONNEMENT { get; set; }
[DataMember]
		public DateTime? DATESTATUTCONTRAT { get; set; }
[DataMember]
		public DateTime? DATEPROCHAINCONTROLEPOSTFRAUDE { get; set; }
[DataMember]
		public Guid? CONSTATDUCONTROLEPOSTFRAUDE_ID { get; set; }
[DataMember]
		public DateTime? DATEDERNIERESELECTIONENCAMPAGNE { get; set; }


    }
}
