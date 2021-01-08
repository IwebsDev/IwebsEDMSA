using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsHabilitationMetier:CsPrint
    {
        [DataMember]  public int IDFONCTION { get; set; }
        [DataMember]
        public string  LIBELLEFONCTION { get; set; }
        [DataMember]
        public int IDPROFIL { get; set; }
        [DataMember]
        public string LIBELLEPROFIL { get; set; }
        public  Nullable<int>  IDCENTRE { get; set; }
        [DataMember]
        public string LIBELLECENTRE { get; set; }
        [DataMember] 
        public Nullable<System.DateTime> DATEDEBUTVALIDITE { get; set; }
        [DataMember] 
        public Nullable<System.DateTime> DATEFINVALIDITE { get; set; }
            

    }
}
