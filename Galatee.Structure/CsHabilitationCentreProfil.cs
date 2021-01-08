using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsHabilitationCentreProfil:CsPrint
    {


        [DataMember]
        public int PK_ID;
        [DataMember]
        public int FK_IDPROFIL;
        [DataMember]
        public int FK_IDCENTRE;
        
        [DataMember] public Nullable<System.DateTime> DATEDEBUTVALIDITE { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEFINVALIDITE { get; set; }
            

    }
}
