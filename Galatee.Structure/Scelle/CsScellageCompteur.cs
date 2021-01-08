using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
    public class CsScellageCompteur
    {
         
        [DataMember]  public string  CapotMoteur_ID_Scelle1 { get; set; }
        [DataMember]  public string CapotMoteur_ID_Scelle2 { get; set; }
        [DataMember]  public string CapotMoteur_ID_Scelle3 { get; set; }
        [DataMember]  public string Cache_Scelle { get; set; }

         
     }
         
}
