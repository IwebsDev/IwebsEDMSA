using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCentreProfil
    {
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDADMUTILISATEUR { get; set; }
       [DataMember] public int FK_IDPROFIL { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public int FK_IDSITE { get; set; }
       [DataMember] public System.DateTime DATEDEBUTVALIDITE { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEFINVALIDITE { get; set; }

       [DataMember] public string  LIBELLECENTRE { get; set; }

    }
}
