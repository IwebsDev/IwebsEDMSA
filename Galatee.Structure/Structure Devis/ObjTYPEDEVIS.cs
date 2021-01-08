using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class ObjTYPEDEVIS : CsPrint
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public string CODEPRODUIT { get; set; }
        [DataMember] public string TDEM { get; set; }
        [DataMember] public string LIBELLETDEM { get; set; }
        [DataMember] public string LIBELLEPRODUIT { get; set; }
        [DataMember] public DateTime? DATECREATION { get; set; }
        [DataMember] public DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public int FK_IDTDEM { get; set; }

        public override string ToString()
        {
            return this.LIBELLE;
        }
    }
}
