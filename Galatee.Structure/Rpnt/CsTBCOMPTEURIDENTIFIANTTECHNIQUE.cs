using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBCOMPTEURIDENTIFIANTTECHNIQUE
    {
		
		
[DataMember]
		public Int32 COMPTEURIDENTIFIANTTECHNIQUEID { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public Int64 DERNIERIDENTIFIANTAFFECTE { get; set; }


    }
}
