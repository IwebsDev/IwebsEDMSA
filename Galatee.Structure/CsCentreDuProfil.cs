using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCentreDuProfil
    {

        public CsCentreDuProfil()
            {
                unprofil = new CsProfil();
                lescentres = new List<CsCentre>();
            }

        [DataMember] public CsProfil unprofil;
        [DataMember] public List<CsCentre> lescentres;
        [DataMember] public Nullable<System.DateTime> DATEDEBUTVALIDITE { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEFINVALIDITE { get; set; }
            

    }
}
