using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsElementsLotDeControleCampagneHTA
    {
		
		
[DataMember]
		public Guid Lot_ID { get; set; }
[DataMember]
		public String Libelle_Lot { get; set; }
[DataMember]
		public Int32 StatutLot_ID { get; set; }
[DataMember]
		public Guid Campagne_ID { get; set; }
[DataMember]
		public Int32? Critere_TypeClient { get; set; }
[DataMember]
		public Int32 NbreElementsDuLot { get; set; }
[DataMember]
		public String MatriculeAgentControleur { get; set; }
[DataMember]
		public DateTime? DateFermeture { get; set; }
[DataMember]
		public String MatriculeAgentCreation { get; set; }
[DataMember]
		public Int32? Critere_TypeCompteur { get; set; }
[DataMember]
		public DateTime DateCreation { get; set; }
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public DateTime DateSelection { get; set; }
[DataMember]
		public DateTime DateAffectationLot { get; set; }
[DataMember]
		public Guid? ResultatControle_ID { get; set; }
[DataMember]
		public Int32? ResultatValue { get; set; }
[DataMember]
		public String MatriculeAgentSaisie { get; set; }
[DataMember]
		public String MatriculeAgentControle { get; set; }
[DataMember]
		public DateTime? DateSaisie { get; set; }
[DataMember]
		public DateTime? DateControle { get; set; }
[DataMember]
		public String Commentaires { get; set; }
[DataMember]
		public Int32 Methode_ID { get; set; }
[DataMember]
		public Int32 Debut_PerAA { get; set; }
[DataMember]
		public Int32 Debut_PerMM { get; set; }
[DataMember]
		public Int32 Fin_PerAA { get; set; }
[DataMember]
		public Int32 Fin_PerMM { get; set; }
[DataMember]
		public Double Difference { get; set; }
[DataMember]
		public Double? _MChuteConso_ConsoEnChute_Valeur { get; set; }
[DataMember]
		public Int32? _MChuteConso_ConsoEnChute_PerAA { get; set; }
[DataMember]
		public Int32? _MChuteConso_ConsoEnChute_PerMM { get; set; }
[DataMember]
		public Int32? _MChuteConso_ConsoPrecedente_PerAA { get; set; }
[DataMember]
		public Int32? _MChuteConso_ConsoPrecedente_PerMM { get; set; }


    }
}
