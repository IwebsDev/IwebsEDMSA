using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsListeControlesPostFraudeBTAEchus
    {
		
		
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public String Branchement_ID { get; set; }
[DataMember]
		public DateTime? DateProchainControlePostFraude { get; set; }
[DataMember]
		public DateTime? DateDernierControle { get; set; }
[DataMember]
		public Int32 TypeAnomalie_ID { get; set; }
[DataMember]
		public String MatriculeAgentSaisie { get; set; }
[DataMember]
		public DateTime? DateSaisie { get; set; }
[DataMember]
		public Guid? ResultatDernierControle_ID { get; set; }
[DataMember]
		public String ActionsDurantLeConstat { get; set; }
[DataMember]
		public String MatriculeAgentControleur { get; set; }
[DataMember]
		public Guid ConstatFraude_ID { get; set; }
[DataMember]
		public String Numero_AvisDePassage { get; set; }


    }
}
