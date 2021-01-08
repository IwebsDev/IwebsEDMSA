using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTourneeAccessiblesParlUO
    {
		
		
[DataMember]
		public Guid Tournee_Id { get; set; }
[DataMember]
		public String CodeTournee { get; set; }
[DataMember]
		public String CodeExploitation { get; set; }
[DataMember]
		public String CodeZone { get; set; }
[DataMember]
		public String LibelleTournee { get; set; }
[DataMember]
		public String CodeUO { get; set; }


    }
}
