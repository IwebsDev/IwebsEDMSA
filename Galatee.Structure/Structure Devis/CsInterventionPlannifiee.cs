using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsInterventionPlannifiee
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public DateTime DATERENDEZVOUS { get; set; }
        [DataMember]
        public string INTERVENTION { get; set; }
        [DataMember]
        public string FK_MATRICULERESPONSABLE { get; set; }
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
