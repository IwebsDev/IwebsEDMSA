using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
      [DataContract]
    public class CsRefStatutsScelles
    {
        [DataMember] public int Status_ID { get; set; }
        [DataMember] public string Status { get; set; }
    }
}
