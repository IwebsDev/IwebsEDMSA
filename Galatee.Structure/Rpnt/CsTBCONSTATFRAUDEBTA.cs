using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBCONSTATFRAUDEBTA
    {
		
		
[DataMember]
		public Guid CONSTATFRAUDE_ID { get; set; }
[DataMember]
public Guid CONSTATFRAUD_PREC_ID { get; set; }
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
		public Guid? CAMPAGNE_ID { get; set; }
[DataMember]
		public Guid? CONTRAT_ID { get; set; }
[DataMember]
		public Boolean? PRESENCE_FORCEORDRE { get; set; }
[DataMember]
		public String ACTIONSDURANTLECONSTAT { get; set; }
[DataMember]
		public Guid? ID_FICHESCANNEE { get; set; }
         [DataMember]
public Nullable<bool> ESTFRAUDE { get; set; }
        [DataMember]
         public Nullable<bool> ESTISOLE { get; set; }
         [DataMember]
public Nullable<int> BRT_ID { get; set; }
[DataMember]
		public Guid? RESULTATCONTROLEPOSTFRAUDE_ID { get; set; }
[DataMember]
public List< CsREFFAMILLEANOMALIEBTA> LISTFAMILLEANOMALIEBTA { get; set; }
[DataMember]
public CsTBLOTDECONTROLEBTA LOTDECONTROLEBTA { get; set; }
    }
}
