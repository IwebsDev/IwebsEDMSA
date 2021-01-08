using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsStatistiquesElementsLotDeControleDeCampagneBTA
    {
		
		
[DataMember]
		public Guid Campagne_ID { get; set; }
[DataMember]
		public Int32? NbreElementsDansDesLotsDeControle { get; set; }
[DataMember]
		public Int32? NbreElementsNonControles { get; set; }
[DataMember]
		public Int32? NbreElementsControlesRAS { get; set; }
[DataMember]
		public Int32? NbreElementsControlesKO { get; set; }


    }
}
