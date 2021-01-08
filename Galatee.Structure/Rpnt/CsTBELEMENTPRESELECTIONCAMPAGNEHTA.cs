using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBELEMENTPRESELECTIONCAMPAGNEHTA
    {
		
		
[DataMember]
		public Guid CAMPAGNE_ID { get; set; }
[DataMember]
		public Guid CONTRAT_ID { get; set; }
[DataMember]
		public DateTime DATESELECTION { get; set; }
[DataMember]
		public Int32 METHODE_ID { get; set; }
[DataMember]
		public Int32 DEBUT_PERAA { get; set; }
[DataMember]
		public Int32 DEBUT_PERMM { get; set; }
[DataMember]
		public Int32 FIN_PERAA { get; set; }
[DataMember]
		public Int32 FIN_PERMM { get; set; }
[DataMember]
		public Double DIFFERENCE { get; set; }
[DataMember]
		public Double? _MCHUTECONSO_CONSOENCHUTE_VALEUR { get; set; }
[DataMember]
		public Int32? _MCHUTECONSO_CONSOENCHUTE_PERAA { get; set; }
[DataMember]
		public Int32? _MCHUTECONSO_CONSOENCHUTE_PERMM { get; set; }
[DataMember]
		public Int32? _MCHUTECONSO_CONSOPRECEDENTE_PERAA { get; set; }
[DataMember]
		public Int32? _MCHUTECONSO_CONSOPRECEDENTE_PERMM { get; set; }


    }
}
