using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
       [DataContract] 
  public  class CsOrganeFraude
    {
        [DataMember] 
        public byte PK_ID { get; set; }
        [DataMember] 
        public string Code { get; set; }
        [DataMember] 
        public string Libelle { get; set; }
    
    }
}
