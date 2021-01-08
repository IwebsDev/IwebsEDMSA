using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Galatee.Structure

{
    [DataContract]
    public class ObjAPPAREILSDEVIS
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public String NUMDEVIS { get; set; }
        [DataMember] public int FK_IDDEVIS { get; set; }
        [DataMember] public String DESIGNATION { get; set; }
        [DataMember] public string CODEAPPAREIL { get; set; }
        [DataMember] public int FK_IDCODEAPPAREIL { get; set; }
        [DataMember] public int? NBRE { get; set; }
        [DataMember] public int? PUISSANCE { get; set; }
        [DataMember]public int TEMPSUTILISATION { get; set; }
        [DataMember] public DateTime? DATECREATION { get; set; }
        [DataMember] public DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
    }
}
