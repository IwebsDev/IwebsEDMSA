using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsHistoriqueCFIsole
    {
		
		
[DataMember]
		public Guid ConstatFraude_ID { get; set; }
[DataMember]
		public String Numero_AvisDePassage { get; set; }
[DataMember]
		public DateTime? DateSaisie { get; set; }
[DataMember]
		public String MatriculeAgentSaisie { get; set; }
[DataMember]
		public DateTime? DateControle { get; set; }
[DataMember]
		public String MatriculeAgentControleur { get; set; }
[DataMember]
		public Guid? Campagne_ID { get; set; }
[DataMember]
		public Boolean? Presence_ForceOrdre { get; set; }
[DataMember]
		public String ActionsDurantLeConstat { get; set; }
[DataMember]
		public Guid? Id_FicheScannee { get; set; }
[DataMember]
		public Binary ContenuFicheScannee { get; set; }
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public Int32? TypeContratID { get; set; }
[DataMember]
		public String Branchement_ID { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public Guid? Huissier_ID { get; set; }
[DataMember]
		public Boolean? Paye { get; set; }
[DataMember]
		public String N_Agrement { get; set; }
[DataMember]
		public String NomHuissier { get; set; }
[DataMember]
		public String PrenomsHuissier { get; set; }
[DataMember]
		public String DisplayName { get; set; }
[DataMember]
		public String N_CompteContribuable { get; set; }
[DataMember]
		public Decimal? TarifPrestations { get; set; }
[DataMember]
		public DateTime? DateProchainControlePostFraude { get; set; }


    }
}
