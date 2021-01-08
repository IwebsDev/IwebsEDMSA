using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsCriterecampagne : CsPrint
    {
        [DataMember] public int IDCONSO { get; set; }
        [DataMember] public int IDTOURNEE { get; set; }
        [DataMember] public int IDCATEGORIE { get; set; }
        [DataMember] public string REFERENCEDEBUT { get; set; }
        [DataMember] public string REFERENCEFIN { get; set; }
        [DataMember] public string ORDRETOURNEEDEBUT { get; set; }
        [DataMember] public string ORDRETOURNEEFIN { get; set; }
        [DataMember] public bool  RESILIERINCLUS { get; set; }
        [DataMember] public DateTime  DATEEXIGIBLE { get; set; }
        [DataMember] public int NOMBREMINIMUMFACTUREDUE { get; set; }
        [DataMember] public decimal  MONTANTMINIMUMFACTUREDUE { get; set; }
        [DataMember] public string PERIODEDUE  { get; set; }
        [DataMember] public string MATRICULE  { get; set; }
    }
 }









