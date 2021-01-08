using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBENSEMBLETECHNIQUE
    {
		
		
[DataMember]
		public String ID_ENSEMBLETECHNIQUE { get; set; }
[DataMember]
		public String LIBELLE_ENSEMBLETECHNIQUE { get; set; }


    }
}
