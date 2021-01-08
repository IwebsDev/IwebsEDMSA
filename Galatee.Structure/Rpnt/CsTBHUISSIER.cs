using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBHUISSIER
    {
		
		
[DataMember]
		public Guid HUISSIER_ID { get; set; }
[DataMember]
		public String N_AGREMENT { get; set; }
[DataMember]
		public String NOM { get; set; }
[DataMember]
		public String PRENOMS { get; set; }
[DataMember]
		public String DISPLAYNAME { get; set; }
[DataMember]
		public String N_COMPTECONTRIBUABLE { get; set; }
[DataMember]
		public Decimal? TARIFPRESTATIONS { get; set; }


    }
}
