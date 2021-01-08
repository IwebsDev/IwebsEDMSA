using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTypeComptage : CsPrint
    {
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string CODE { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public bool TRANSFO_UNIQUE { get; set; }
        [DataMember] public int PUISSANCEINSTALLEE_MINI { get; set; }
        [DataMember] public int PUISSANCEINSTALLEE_MAXI { get; set; }
        [DataMember] public bool AVEC_PERTE { get; set; }
        [DataMember] public bool AVEC_PRIMEFIXE { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public int PK_ID { get; set; }
    }

}
