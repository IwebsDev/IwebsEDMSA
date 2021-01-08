using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsLigneABAQUE_ChoixTC
    {
		
		
[DataMember]
		public Int32 TypeComptageHTA_ID { get; set; }
[DataMember]
		public Char? NiveauTENSION_ID { get; set; }
[DataMember]
		public String ABAQUE_ID { get; set; }
[DataMember]
		public Double PS_BorneInf { get; set; }
[DataMember]
		public Double PS_BorneSup { get; set; }
[DataMember]
		public Int32 RapportTC_INTENTREE { get; set; }
[DataMember]
		public Int32 RapportTC_INTSORTIE { get; set; }


    }
}
