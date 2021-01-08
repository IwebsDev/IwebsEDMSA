using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsPeriodeFacturationLaPlusRecentePourLesBET
    {
		
		
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public Int32? AnneeDernierePeriode { get; set; }
[DataMember]
		public Int32? MoisDernierePeriode { get; set; }


    }
}
