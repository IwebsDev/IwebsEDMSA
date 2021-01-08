using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsStatFactRecap : CsPrint
    {
        [DataMember]public string CENTRE { get; set; }
        [DataMember]public string LIBELLECENTRE { get; set; }

        [DataMember] public Int64  CONSOFACT { get; set; }
        [DataMember] public Int64  CONSO  { get; set; }
        [DataMember] public Int64  NOMBRE  { get; set; }
        [DataMember]public string LIBELLEREDEVANCE { get; set; }
        [DataMember]public string LIBELLECATEGORIE { get; set; }

        [DataMember] public decimal MONTANTHT { get; set; }
        [DataMember] public decimal MONTANTTAXE { get; set; }
        [DataMember] public decimal MONTANTTTC { get; set; }
    }
}
