using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsREFTYPEABONNE
    {
		
		
[DataMember]
         public String TYPABON_ID { get; set; }
[DataMember]
		public String TYPABON_LIBELLE { get; set; }


    }
}
