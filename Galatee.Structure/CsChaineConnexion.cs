using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsChaineConnexion
    {
        

        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Valeur { get; set; }
    }
}
