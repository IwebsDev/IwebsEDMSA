using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsStatistiquesCampagneBTA
    {
		
		
[DataMember]
		public Guid Campagne_ID { get; set; }
[DataMember]
		public String Libelle_Campagne { get; set; }
[DataMember]
		public String CodeUO { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public Int32 Statut_ID { get; set; }
[DataMember]
		public DateTime DateCreation { get; set; }
[DataMember]
		public Int32 NbreElements { get; set; }
[DataMember]
		public DateTime DateDebutControles { get; set; }
[DataMember]
		public DateTime DateFinPrevue { get; set; }
[DataMember]
		public Int32 NbreElementsControles_RAS { get; set; }
[DataMember]
		public Int32 NbreElementsControles_KO { get; set; }


    }
}
