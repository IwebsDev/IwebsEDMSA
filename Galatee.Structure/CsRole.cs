using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    class CsRole: CsPrint
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public string CODE { get; set; }
        [DataMember]
        public string ROLENAME { get; set; }
        [DataMember]
        public string ROLEDISPLAYNAME { get; set; }
        [DataMember]
        public bool ESTADMIN { get; set; }
        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
    }
}
