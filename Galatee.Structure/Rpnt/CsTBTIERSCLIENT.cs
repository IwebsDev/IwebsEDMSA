using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBTIERSCLIENT
    {
		
		
[DataMember]
		public String REFERENCECLIENT { get; set; }
[DataMember]
		public Int32? TYPECLIENT { get; set; }
[DataMember]
		public String NOM { get; set; }
[DataMember]
		public String PRENOMS { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public Char? TYPABON_ID { get; set; }


    }
}
