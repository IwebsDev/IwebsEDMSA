using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDetailTarifFacturation:CsPrint
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int FK_IDTARIFFACTURATION { get; set; }
        [DataMember] public int FK_IDREDEVANCE { get; set; }
        [DataMember] public byte NUMEROTRANCHE { get; set; }
        [DataMember] public int QTEANNUELMAXI { get; set; }
        [DataMember] public Nullable<decimal> PRIXUNITAIRE { get; set; }
        [DataMember] public bool IsNew { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }

    }
}
