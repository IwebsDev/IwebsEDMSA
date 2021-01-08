using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{


    [DataContract]
    public class CsPoliceDispo
    {
        [DataMember]
        public string   CENTRE { get; set; }
        [DataMember]
        public string AG { get; set; }

    }

}
