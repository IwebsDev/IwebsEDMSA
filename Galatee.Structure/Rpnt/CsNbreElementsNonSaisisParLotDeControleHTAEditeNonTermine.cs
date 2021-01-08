using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsNbreElementsNonSaisisParLotDeControleHTAEditeNonTermine
    {
		
		
[DataMember]
		public Guid Campagne_ID { get; set; }
[DataMember]
		public Guid Lot_ID { get; set; }
[DataMember]
		public Int32? NbreElementsNonControles { get; set; }
[DataMember]
		public Int32 StatutLot_ID { get; set; }


    }
}
