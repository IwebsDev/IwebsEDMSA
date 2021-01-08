using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBCONSTATFRAUDEHTA
    {
		
		
[DataMember]
		public Guid CONSTATFRAUDE_ID { get; set; }
[DataMember]
		public String NUMERO_AVISDEPASSAGE { get; set; }
[DataMember]
		public DateTime? DATESAISIE { get; set; }
[DataMember]
		public String MATRICULEAGENTSAISIE { get; set; }
[DataMember]
		public DateTime? DATECONTROLE { get; set; }
[DataMember]
		public String MATRICULEAGENTCONTROLEUR { get; set; }
[DataMember]
		public Guid? CAMPAGNEHTA_ID { get; set; }
[DataMember]
		public Guid? CONTRATHTA_ID { get; set; }
[DataMember]
		public Boolean? PRESENCE_FORCEORDRE { get; set; }
[DataMember]
		public String ACTIONSDURANTLECONSTAT { get; set; }
[DataMember]
		public Guid? ID_FICHESCANNEE { get; set; }
[DataMember]
		public Guid? RESULTATCONTROLEPOSTFRAUDE_ID { get; set; }


    }
}
