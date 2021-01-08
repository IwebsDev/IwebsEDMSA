using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDonneeSite
    {
        [DataMember] public CsSite  _LeSite { get; set; }
        [DataMember] public CsCentre   _LeCentre { get; set; }
        [DataMember] public CsSpeSite    _LaSpecialiteSite { get; set; }
    }
}
