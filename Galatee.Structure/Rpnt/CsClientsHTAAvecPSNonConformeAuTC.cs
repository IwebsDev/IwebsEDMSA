using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsClientsHTAAvecPSNonConformeAuTC
    {
		
		
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public Int32? TypeComptageHTA { get; set; }
[DataMember]
		public Double? PuissanceSouscrite { get; set; }
[DataMember]
		public Char? NiveauTENSION_ID { get; set; }
[DataMember]
		public Int32? TCClient_INTENTREE { get; set; }
[DataMember]
		public Int32? TCClient_INTSORTIE { get; set; }
[DataMember]
		public String ABAQUE_ID { get; set; }
[DataMember]
		public Int32 AbaqueTC_INTENTREE { get; set; }
[DataMember]
		public Int32 AbaqueTC_INTSORTIE { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public Int32 StatutContrat { get; set; }
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public String Raccordement_ID { get; set; }
[DataMember]
		public String TypeComptageHTA_Libelle { get; set; }


    }
}
