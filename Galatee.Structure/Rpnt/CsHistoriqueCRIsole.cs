using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsHistoriqueCRIsole
    {
		
		
[DataMember]
		public Guid ResultatControle_ID { get; set; }
[DataMember]
		public Int32 ResultatValue { get; set; }
[DataMember]
		public String MatriculeAgentSaisie { get; set; }
[DataMember]
		public String MatriculeAgentControle { get; set; }
[DataMember]
		public DateTime DateSaisie { get; set; }
[DataMember]
		public DateTime DateControle { get; set; }
[DataMember]
		public String Commentaires { get; set; }
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public Int32 StatutContrat { get; set; }
[DataMember]
		public Int32? TypeContratID { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String Numero_Compteur { get; set; }
[DataMember]
		public String Branchement_ID { get; set; }
[DataMember]
		public Int32? TypeBranchement_ID { get; set; }
[DataMember]
		public Int32? TypeClient { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public Binary ContenuFicheScannee { get; set; }
[DataMember]
		public String CodeUO { get; set; }


    }
}
