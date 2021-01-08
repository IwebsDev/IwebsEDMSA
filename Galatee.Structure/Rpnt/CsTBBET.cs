using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBBET
    {
		
		
[DataMember]
		public Int64 IDENTIFIANTBET { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public Int32? PERIODEFACT_AN { get; set; }
[DataMember]
		public Int32? PERIODEFACT_MOIS { get; set; }
[DataMember]
		public String BRANCHEMENT_ID { get; set; }
[DataMember]
		public Guid? CONTRATBTA_ID { get; set; }
[DataMember]
		public String MATRICULEAGENTZONE { get; set; }
[DataMember]
		public Boolean? TRAITEMENT_BET { get; set; }
[DataMember]
		public String MATRICULEAGENTMODIFICATION { get; set; }
[DataMember]
		public String NUMERO_COMPTEUR { get; set; }
[DataMember]
		public Int32? CONSOMMATION { get; set; }
[DataMember]
		public Int32? CONSOMOYENNE { get; set; }
[DataMember]
		public String INDEXRELA { get; set; }
[DataMember]
		public Int32? INDEXNVL { get; set; }
[DataMember]
		public String CODANO_1 { get; set; }
[DataMember]
		public String CODANO_2 { get; set; }
[DataMember]
		public String CODANO_3 { get; set; }
[DataMember]
		public String CODANO_4 { get; set; }
[DataMember]
		public String CODANO_5 { get; set; }
[DataMember]
		public String COMMENTAIRES { get; set; }


    }
}
