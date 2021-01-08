using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsEditionCRNonSaisi
    {
		
		
[DataMember]
		public String Libelle_Campagne { get; set; }
[DataMember]
		public String CodeUO { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public Guid Lot_ID { get; set; }
[DataMember]
		public String Libelle_Lot { get; set; }
[DataMember]
		public Int32 StatutLot_ID { get; set; }
[DataMember]
		public Guid Campagne_ID { get; set; }
[DataMember]
		public DateTime DateCreation { get; set; }
[DataMember]
		public String MatriculeAgentControleur { get; set; }
[DataMember]
		public Guid? ResultatControle_ID { get; set; }
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public String Branchement_ID { get; set; }
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public Int32 Methode_ID { get; set; }
[DataMember]
		public String Libele_Methode { get; set; }
[DataMember]
		public Guid Expr1 { get; set; }


    }
}
