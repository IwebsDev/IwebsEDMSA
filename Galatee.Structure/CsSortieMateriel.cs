using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsSortieMateriel
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int FK_IDDEMANDE { get; set; }
        [DataMember] public int FK_IDTYPEMATERIEL { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember]  public string NOMBRE { get; set; }
        [DataMember] public string LIVRE { get; set; }
        [DataMember] public string RECU { get; set; }
        [DataMember] public Nullable<int> FK_IDLIVREUR { get; set; }
        [DataMember] public Nullable<int> FK_IDRECEPTEUR { get; set; }
        [DataMember] public System.DateTime DATELIVRAISON { get; set; }
        [DataMember] public Nullable<System.DateTime> DATERECEPTION { get; set; }

        [DataMember] public string LIVREUR { get; set; }
        [DataMember] public string RECEPTEUR { get; set; }
        [DataMember] public string NUMDEM { get; set; }

    }
}
