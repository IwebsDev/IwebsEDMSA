using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{


    [DataContract]
    public class CsTarifClient : CsPrint
    {
        [DataMember] public string REDEVANCE { get; set; }
        [DataMember] public byte TRANCHE { get; set; }
        [DataMember] public string PLAGE { get; set; }
        [DataMember] public decimal  PRIXUNITAIRE { get; set; }
        [DataMember] public string UNITE { get; set; }
        [DataMember] public string TYPEREDEVANCE { get; set; }

    }

}
