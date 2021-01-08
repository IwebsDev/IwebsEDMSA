using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsArrete
    {
        [DataMember]
        public string ANNMOIS { get; set; }
        [DataMember]
        public DateTime? DATEARRETE { get; set; }
        [DataMember]   
        public DateTime? DATEDEBUT { get; set; }
        [DataMember]   
        public DateTime? DATEFIN { get; set; }
        [DataMember]
        public string STATUT { get; set; }
        [DataMember]
        public string ETAPE { get; set; }
        [DataMember]
        public string CUMULETAPE { get; set; }
        [DataMember]
        public string MOISCOMPTABLE { get; set; }
        [DataMember]
        public bool COCHER { get; set; }
    }
}
