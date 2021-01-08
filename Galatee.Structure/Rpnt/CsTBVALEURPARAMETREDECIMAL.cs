using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBVALEURPARAMETREDECIMAL
    {
		
		
[DataMember]
		public Int32 PARAMETRE_ID { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public Decimal VALEUR { get; set; }


    }
}
