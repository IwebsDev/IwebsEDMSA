using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsREFSIEGEANOMALIE
    {
		
		
[DataMember]
		public Int32 SIEGEANOMALIE_ID { get; set; }
[DataMember]
		public String SIEGEANOMALIE_LIBELLE { get; set; }


    }
}
