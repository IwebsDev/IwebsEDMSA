using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsREFUSAGEABONNEMENT
    {
		
		
[DataMember]
		public Char USAGEABONNEMENT_ID { get; set; }
[DataMember]
		public String LIBELLEUSAGE { get; set; }


    }
}
