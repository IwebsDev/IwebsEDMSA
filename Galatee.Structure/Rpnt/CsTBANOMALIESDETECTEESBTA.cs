using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBANOMALIESDETECTEESBTA
    {
		
		
[DataMember]
		public Guid RESULTATCONTROLE_ID { get; set; }

[DataMember]
public System.Guid ANOMALIESDETECTEES_ID { get; set; }
         [DataMember]
public int TYPEANOMALIE_ID { get; set; }
         [DataMember]
public string LIBELLEANOMALIE { get; set; }
         [DataMember]
public Nullable<int> RESULTATVALUE { get; set; }
         [DataMember]
public string MATRICULEAGENTSAISIE { get; set; }
         [DataMember]
public string MATRICULEAGENTCONTROLE { get; set; }
         [DataMember]
public Nullable<System.DateTime> DATESAISIE { get; set; }
         [DataMember]
public Nullable<System.DateTime> DATECONTROLE { get; set; }
         [DataMember]
public Nullable<int> TYPEDECONTROLE { get; set; }
         [DataMember]
public string CODEEXPLOITATION { get; set; }
         [DataMember]
public string COMMENTAIRES { get; set; }
         [DataMember]
public Nullable<System.Guid> CONSTATFRAUDE_ID { get; set; }
         [DataMember]
public Nullable<System.Guid> ID_FICHEVERIFICATIONSCANNEE { get; set; }
         [DataMember]
public int BRT_ID { get; set; }
         [DataMember]
public System.Guid LOT_ID { get; set; }

         [DataMember]
public virtual CsREFTYPEANOMALIEBTA REFTYPEANOMALIEBTA { get; set; }
         [DataMember]
public virtual CsTBCONSTATFRAUDEBTA TBCONSTATFRAUDEBTA { get; set; }
    }
}
