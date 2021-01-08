using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsAbonneHTAAvecRaccordement
    {
		
		
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public Int32? TypeClient { get; set; }
[DataMember]
		public Char? TYPABON_ID { get; set; }
[DataMember]
		public String Raccordement_ID { get; set; }
[DataMember]
		public Int32? TypeComptageHTA { get; set; }
[DataMember]
		public Int32 StatutContrat { get; set; }
[DataMember]
		public Double? PuissanceSouscrite { get; set; }
[DataMember]
		public String TypeTarif_ID { get; set; }
[DataMember]
		public Int32? RapportTC_INTENTREE { get; set; }
[DataMember]
		public Int32? RapportTC_INTSORTIE { get; set; }
[DataMember]
		public Char? NiveauTENSION_ID { get; set; }
[DataMember]
		public Guid Contrat_ID { get; set; }
[DataMember]
		public Guid? ResultatDernierControle_ID { get; set; }
[DataMember]
		public DateTime? DateProchainControlePostFraude { get; set; }


    }
}
