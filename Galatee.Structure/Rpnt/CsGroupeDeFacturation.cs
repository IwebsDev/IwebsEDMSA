using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsGroupeDeFacturation
    {
		
		
[DataMember]
		public Int32? GroupeDeFacturation { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String Libelle { get; set; }


    }
}
