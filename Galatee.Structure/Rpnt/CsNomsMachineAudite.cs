using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
     [DataContract]
    public class CsNomsMachineAudite
    {
		
		
[DataMember]
		public String NomMachineValue { get; set; }
[DataMember]
		public String NomMachineLibelle { get; set; }


    }
}
