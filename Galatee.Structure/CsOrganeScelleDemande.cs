using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsOrganeScelleDemande
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public string NUM_SCELLE { get; set; }
        [DataMember] public string LIBELLEORGANE_SCELLABLE { get; set; }
        [DataMember]  public Nullable<int> FK_IDORGANE_SCELLABLE { get; set; }
        [DataMember] public Nullable<int> FK_IDDEMANDE { get; set; }
        [DataMember] public Nullable<int> FK_IDBRT { get; set; }
        [DataMember] public Nullable<int> NOMBRE { get; set; }
        [DataMember] public byte[] CERTIFICAT { get; set; }
        [DataMember] public string  USERCREATION { get; set; }

        [DataMember] public Nullable<int> IDCOULEUR { get; set; }

    }
}
