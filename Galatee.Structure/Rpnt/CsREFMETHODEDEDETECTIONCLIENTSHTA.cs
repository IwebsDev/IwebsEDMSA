using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsREFMETHODEDEDETECTIONCLIENTSHTA
    {
		
		
[DataMember]
		public Int32 METHODE_ID { get; set; }
[DataMember]
		public String LIBELE_METHODE { get; set; }
[DataMember]
		public String DESCRIPTION { get; set; }


    }
}
