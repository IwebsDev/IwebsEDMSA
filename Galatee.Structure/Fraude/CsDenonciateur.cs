using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{  [DataContract]
  public  class CsDenonciateur
    {
    [DataMember] public int PK_ID { get; set; }
    [DataMember] public string Nom { get; set; }
    [DataMember] public string Localisation { get; set; }
    [DataMember] public string Contact { get; set; }
    [DataMember] public string LienAvecAbonne { get; set; }
    [DataMember] public DateTime DateDenonciation { get; set; }
    [DataMember]  public int FK_IDMOYENDENONCIATION { get; set; }
    [DataMember] public int FK_IDLOCALISATION { get; set; }
    
     
    }
}
