using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsREFUNITEORGANISATIONNELLE
    {
		
		
[DataMember]
		public String CODEUO { get; set; }
[DataMember]
		public String LIBELLEUO { get; set; }
[DataMember]
		public Int32 NIVEAUID { get; set; }


    }
}
