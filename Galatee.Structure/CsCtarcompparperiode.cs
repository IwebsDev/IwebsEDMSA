using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsCtarcompparperiode : CsPrint
    {
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public string RECHERCHETARIF { get; set; }
       [DataMember] public int FK_IDCENTRETARIF { get; set; }
       [DataMember] public int FK_IDPRODUIT { get; set; }
       [DataMember] public int FK_IDABON { get; set; }
       [DataMember] public string LOTRI { get; set; }
       [DataMember] public string PERIODE { get; set; }
       [DataMember] public string CTARCOMP { get; set; }

    }
 }









