using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsBordereauControlesPostFraudeBTA
    {
		
		
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public Int32 StatutContrat { get; set; }
[DataMember]
		public DateTime? DateControleInitial { get; set; }
[DataMember]
		public DateTime? DateProchainControlePostFraude { get; set; }
[DataMember]
		public String Branchement_ID { get; set; }


    }
}
