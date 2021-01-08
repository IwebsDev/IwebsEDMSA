using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsREFABAQUE_CHOIXTC
    {
		
		
[DataMember]
		public String ABAQUE_ID { get; set; }
[DataMember]
		public String ABAQUE_LIBELLE { get; set; }
[DataMember]
public String NIVEAUTENSION_ID { get; set; }
[DataMember]
public String NIVEAUTENSION_LIBELLE { get; set; }
[DataMember]
public string TYPECOMPTAGEHTA_LIBELLE { get; set; }
[DataMember]
public List<CsREFLIGNEABAQUE_CHOIXTC> LIST_REFLIGNEABAQUE_CHOIXTC { get; set; }
[DataMember]
public int TRANSAC_STATE { get; set; }
    }
}
