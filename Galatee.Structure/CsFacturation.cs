using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsFacturation : CsPrint
    {
        [DataMember] public List<CsLotri> LOTRI { get; set; }
        [DataMember] public List<CsEntfac> laFacture { get; set; }
        [DataMember] public List<CsEvenement> lesEvenement { get; set; }
        [DataMember] public List<CsAnnomalie> lesAnnomalie { get; set; }
        
    }
}
