using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsRechercheBranchementBTA
    {
		
		
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public Int32? TypeBranchement_ID { get; set; }
[DataMember]
		public String Branchement_ID { get; set; }
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public Int32 StatutContrat { get; set; }
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public String Statut_Branchement { get; set; }


    }
}
