using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsConstatFraudeBTA_Huissier
    {
		
		
[DataMember]
		public Guid ConstatFraude_ID { get; set; }
[DataMember]
		public Guid? Huissier_ID { get; set; }
[DataMember]
		public String Numero_AvisDePassage { get; set; }
[DataMember]
		public DateTime? DateSaisie { get; set; }
[DataMember]
		public String MatriculeAgentSaisie { get; set; }
[DataMember]
		public String MatriculeAgentControleur { get; set; }
[DataMember]
		public Guid? Contrat_ID { get; set; }
[DataMember]
		public DateTime? DateControle { get; set; }
[DataMember]
		public String ActionsDurantLeConstat { get; set; }
[DataMember]
		public Int32 TypeAnomalie_ID { get; set; }
[DataMember]
		public Boolean? Presence_ForceOrdre { get; set; }
[DataMember]
		public Binary ContenuFicheScannee { get; set; }


    }
}
