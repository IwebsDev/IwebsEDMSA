using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBTOURNEE
    {
		
		
[DataMember]
		public Guid TOURNEE_ID { get; set; }
[DataMember]
		public String CODETOURNEE { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public String CODEZONE { get; set; }
[DataMember]
		public String LIBELLETOURNEE { get; set; }
[DataMember]
		public String MATRICULEAZ { get; set; }
[DataMember]
		public Int32? NUMGF { get; set; }


    }
}
