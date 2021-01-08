using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsOrigineLot : CsPrint
    {
        [DataMember]
        public string FK_SITE { get; set; }
        [DataMember]
        public string ORIGINE { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
               [DataMember]
        public int PK_ID { get; set; }
        
    }
 }









