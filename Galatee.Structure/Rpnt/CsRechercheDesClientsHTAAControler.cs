using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsRechercheDesClientsHTAAControler
    {
		
		
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public Int32? TypeClient { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public Int32 StatutContrat { get; set; }
[DataMember]
		public String Numero_Compteur { get; set; }
[DataMember]
		public String Raccordement_ID { get; set; }
[DataMember]
		public Double? PuissanceSouscrite { get; set; }
[DataMember]
		public String TypeTarif_ID { get; set; }
[DataMember]
		public Int32? TypeComptageHTA { get; set; }
[DataMember]
		public String ID_EnsembleTechnique { get; set; }
[DataMember]
		public Guid? ResultatDernierControle_ID { get; set; }
[DataMember]
		public Int32? ResultatValue { get; set; }
[DataMember]
		public String MatriculeAgentControle { get; set; }
[DataMember]
		public DateTime? DateControle { get; set; }
[DataMember]
		public DateTime? DateDerniereSelectionEnCampagne { get; set; }


    }
}
