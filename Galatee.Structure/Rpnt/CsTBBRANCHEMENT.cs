using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBBRANCHEMENT
    {
		
		
[DataMember]
		public String BRANCHEMENT_ID { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public String NUMERO_COMPTEUR { get; set; }
[DataMember]
		public Int32? TYPEBRANCHEMENT_ID { get; set; }
[DataMember]
		public String STATUT_BRANCHEMENT { get; set; }
[DataMember]
		public Guid? TOURNEE_ID { get; set; }
[DataMember]
		public Guid? CONTRATCOURANT_ID { get; set; }


    }
}
