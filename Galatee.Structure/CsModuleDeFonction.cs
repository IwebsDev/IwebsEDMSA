using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
    public class CsModuleDeFonction : CsPrint
    {
       [DataMember] public int FK_IDFONCTION { get; set; }
       [DataMember] public int FK_IDMODULE { get; set; }
       [DataMember] public int PK_ID { get; set;} 
    }
}
