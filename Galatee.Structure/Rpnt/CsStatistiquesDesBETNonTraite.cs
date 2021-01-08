using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsStatistiquesDesBETNonTraite
    {
		
		
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String CodeUO { get; set; }
[DataMember]
		public Int32? Periodefact_An { get; set; }
[DataMember]
		public Int32? PeriodeFact_Mois { get; set; }
[DataMember]
		public Int64 IdentifiantBET { get; set; }
[DataMember]
		public String Branchement_ID { get; set; }
[DataMember]
		public Guid? ContratBTA_ID { get; set; }
[DataMember]
		public String MatriculeAgentZone { get; set; }
[DataMember]
		public String NomAgentIntervention { get; set; }
[DataMember]
		public String PrenomsAgentIntervention { get; set; }
[DataMember]
		public String MatriculeAgentModification { get; set; }
[DataMember]
		public String NomAgentModification { get; set; }
[DataMember]
		public String PrenomsAgentModification { get; set; }
[DataMember]
		public Boolean? Traitement_BET { get; set; }
[DataMember]
		public String Numero_Compteur { get; set; }
[DataMember]
		public Int32? Consommation { get; set; }
[DataMember]
		public Int32? ConsoMoyenne { get; set; }
[DataMember]
		public String IndexRela { get; set; }
[DataMember]
		public Int32? IndexNvl { get; set; }
[DataMember]
		public String codano_1 { get; set; }
[DataMember]
		public String codano_2 { get; set; }
[DataMember]
		public String codano_3 { get; set; }
[DataMember]
		public String codano_4 { get; set; }
[DataMember]
		public String codano_5 { get; set; }
[DataMember]
		public String LibelleCodeAno1 { get; set; }
[DataMember]
		public String LibelleCodeAno2 { get; set; }
[DataMember]
		public String LibelleCodeAno3 { get; set; }
[DataMember]
		public String LibelleCodeAno4 { get; set; }
[DataMember]
		public String LibelleCodeAno5 { get; set; }
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public Int32 StatutContrat { get; set; }
[DataMember]
		public Int32? GroupeDeFacturation { get; set; }
[DataMember]
		public Double? PuissanceSouscrite { get; set; }
[DataMember]
		public DateTime? DateAbonnement { get; set; }
[DataMember]
		public DateTime? DateStatutContrat { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public Char? TYPABON_ID { get; set; }
[DataMember]
		public String TYPABON_Libelle { get; set; }
[DataMember]
		public String Commentaires { get; set; }
[DataMember]
		public String StatutContrat_Libelle { get; set; }


    }
}
