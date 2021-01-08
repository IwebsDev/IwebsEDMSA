using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
    public class CsCompteurScelle
    {

        [DataMember] public CsCompteurBta  LeCompteur { get; set; }
        [DataMember] public CsScellageCompteur  leScellage { get; set; }
         
     }
         
}
