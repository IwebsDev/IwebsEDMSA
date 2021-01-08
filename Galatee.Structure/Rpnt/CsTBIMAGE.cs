using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBIMAGE
    {
		
		
[DataMember]
		public Guid ID_FICHESCANNEE { get; set; }
[DataMember]
		public Binary CONTENUFICHESCANNEE { get; set; }


    }
}
