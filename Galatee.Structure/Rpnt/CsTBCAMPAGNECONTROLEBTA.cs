using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBCAMPAGNECONTROLEBTA
    {
		
		
[DataMember]
		public Guid CAMPAGNE_ID { get; set; }

[DataMember]
		public String LIBELLE_CAMPAGNE { get; set; }

[DataMember]
		public String CODEEXPLOITATION { get; set; }

[DataMember]
public int? CODECENTRE { get; set; }

[DataMember]
public String LIBELLECENTRE { get; set; }

[DataMember]
public String LIBELLEEXPLOITATION { get; set; }

[DataMember]
		public Int32 STATUT_ID { get; set; }

[DataMember]
public String STATUT { get; set; }

[DataMember]
		public String MATRICULEAGENTCREATION { get; set; }

[DataMember]
		public DateTime DATECREATION { get; set; }

[DataMember]
		public Int32 NBREELEMENTS { get; set; }

[DataMember]
		public String MATRICULEAGENTDERNIEREMODIFICATION { get; set; }

[DataMember]
		public DateTime? DATEMODIFICATION { get; set; }

[DataMember]
		public DateTime DATEDEBUTCONTROLES { get; set; }

[DataMember]
		public DateTime DATEFINPREVUE { get; set; }

[DataMember]
        public int NBRLOTS { get; set; }

[DataMember]
        public Int32 POULATIONNONAFFECTES { get; set; }

[DataMember]
public ObservableCollection<CsBrt> LISTEBRANCHEMENT { get; set; }

[DataMember]
public List<CsTBLOTDECONTROLEBTA> LISTELOT { get; set; }

[DataMember]
public String PERIODE { get; set; }

[DataMember]
public CsREFMETHODEDEDETECTIONCLIENTSBTA METHODE { get; set; }
[DataMember]
public int? METHODE_ID { get; set; }

    }
}
