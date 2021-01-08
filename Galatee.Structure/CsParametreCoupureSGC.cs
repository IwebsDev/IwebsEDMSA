using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    [DataContract]
    public class CsParametreCoupureSGC
    {
       [DataMember] public string NOMCHEFSERVICE { get; set; }
       [DataMember] public string NOM_DONNEURORDRE { get; set; }
       [DataMember] public string TITRE_DONNEURORDRE { get; set; }
       [DataMember] public string CONTACT_DONNEURORDRE { get; set; }
       [DataMember] public string STRUCTURE_EXECUTION { get; set; }
       [DataMember] public string AGENT_EXECUTION { get; set; }
       [DataMember] public string MATRICULE_EXECUTION { get; set; }
       [DataMember] public int PK_ID { get; set; }
     }
}
