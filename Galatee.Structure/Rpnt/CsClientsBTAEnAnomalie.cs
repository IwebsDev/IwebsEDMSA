using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsClientsBTAEnAnomalie
    {
		
		
[DataMember]
		public String CodeUO { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String ReferenceClient { get; set; }
[DataMember]
		public String Branchement_ID { get; set; }
[DataMember]
		public Int32? TypeContratID { get; set; }
[DataMember]
		public String TypeTarif_ID { get; set; }
[DataMember]
		public Int32 StatutContrat { get; set; }
[DataMember]
		public Int32 TypeAnomalie_ID { get; set; }
[DataMember]
		public String TypeAnomalie_Libelle { get; set; }
[DataMember]
		public String FamilleAnomalie_Libelle { get; set; }
[DataMember]
		public String Nom { get; set; }
[DataMember]
		public String Prenoms { get; set; }
[DataMember]
		public DateTime DateControle { get; set; }
         [DataMember]
public DateTime Periode { get; set; }
[DataMember]
public decimal ConsoAttendu { get; set; }
[DataMember]
public decimal ConsoFacture { get; set; }
[DataMember]
public decimal DifferenceConso { get; set; }
[DataMember]
public decimal Methode_detection { get; set; }

    }
}
