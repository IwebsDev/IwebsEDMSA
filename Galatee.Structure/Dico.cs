using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class Dico
    {
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public List<CsDetailCampagneGc> Valeur { get; set; }

    }
}
