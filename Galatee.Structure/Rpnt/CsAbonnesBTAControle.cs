using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsAbonnesBTAControle
    {
		
		
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public Int32? TypeClient { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public String Branchement_ID { get; set; }
[DataMember]
		public Int32? TypeContratID { get; set; }
[DataMember]
		public String TypeTarif_ID { get; set; }
[DataMember]
		public Int32 StatutContrat { get; set; }
[DataMember]
		public Guid? ResultatDernierControle_ID { get; set; }
[DataMember]
		public Guid? ResultatControle_ID { get; set; }
[DataMember]
		public Guid? Lot_ID { get; set; }


    }
}
