using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsAffectationGestionnaire:CsPrint
    {
        [DataMember] public int PK { get; set; }
        [DataMember] public Nullable<int> FK_IDADMUTILISATEUR { get; set; }
        [DataMember] public Nullable<int> FK_IDREGROUPEMENT { get; set; }
        [DataMember] public string  LIBELLEREGROUPEMENT { get; set; }
        [DataMember] public string  CODE { get; set; }
        [DataMember] public Nullable<DateTime> DATECREATION { get; set; }
        [DataMember] public Nullable<bool> ISACTIVE { get; set; }
        [DataMember] public string CODECENTRE { get; set; }

    }
}
