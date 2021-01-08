using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBCOMPTEURHTA
    {
		
		
[DataMember]
		public String NUMERO_COMPTEUR { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }


    }
}
