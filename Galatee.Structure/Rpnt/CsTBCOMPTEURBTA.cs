using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBCOMPTEURBTA
    {
		
		
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public String NUMERO_COMPTEUR { get; set; }
[DataMember]
		public Int32? TYPECOMPTEUR_ID { get; set; }
[DataMember]
		public String STATUTCOMPTEUR { get; set; }
[DataMember]
		public Double? COEFLECT { get; set; }


    }
}
