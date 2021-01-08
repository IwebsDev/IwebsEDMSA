using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    class CsModuleDuRole
    {
        [DataMember]
        public int FK_ROLE { get; set; }
        [DataMember]
        public int FK_PROGRAM { get; set; }
    }
}
