using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsTBRACCORDEMENT
    {
		
		
[DataMember]
		public String RACCORDEMENT_ID { get; set; }
[DataMember]
		public String CODEEXPLOITATION { get; set; }
[DataMember]
		public String NUMERO_COMPTEUR { get; set; }
[DataMember]
		public Int32? TYPECOMPTAGEHTA { get; set; }
[DataMember]
		public Guid? CONTRATHTACOURANT_ID { get; set; }
[DataMember]
		public String ID_ENSEMBLETECHNIQUE { get; set; }


    }
}
