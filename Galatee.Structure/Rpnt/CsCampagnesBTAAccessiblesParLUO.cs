using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsCampagnesBTAAccessiblesParLUO:CsPrint
    {
		
		
[DataMember]
		public String CodeUO { get; set; }
[DataMember]
		public Guid Campagne_ID { get; set; }
[DataMember]
		public String Libelle_Campagne { get; set; }
[DataMember]
		public Int32 Statut_ID { get; set; }
[DataMember]
		public String MatriculeAgentCreation { get; set; }
[DataMember]
		public DateTime DateCreation { get; set; }
[DataMember]
		public Int32 NbreElements { get; set; }
[DataMember]
        public Int32 NbreElementsControle { get; set; }
[DataMember]
        public Int32 NbreElementsEnfraud { get; set; }
[DataMember]
		public String MatriculeAgentDerniereModification { get; set; }
[DataMember]
		public DateTime? DateModification { get; set; }
[DataMember]
		public DateTime DateDebutControles { get; set; }
[DataMember]
		public DateTime DateFinPrevue { get; set; }
[DataMember]
		public int? fk_idCentre { get; set; }
[DataMember]
        public String CodeCentre { get; set; }


[DataMember]
public List<Galatee.Structure.Rpnt.CstbLotsDeControleBTA> ListLot { get; set; }

[DataMember]
public List<Galatee.Structure.Rpnt.CsElementsDeCampagneBTA> ListElementsCamp { get; set; }
    }
}
