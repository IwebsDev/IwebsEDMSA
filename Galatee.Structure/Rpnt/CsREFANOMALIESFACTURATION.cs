using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsREFANOMALIESFACTURATION
    {
		
		
[DataMember]
		public String CODEANOMALIE { get; set; }
[DataMember]
		public String LIBELLEANAMOLIE { get; set; }


    }
}
