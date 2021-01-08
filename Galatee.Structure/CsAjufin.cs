using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsAjufin : CsPrint
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public int FK_IDCOPER { get; set; }
        [DataMember]
        public int FK_IDCENTRE { get; set; }
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string CLE { get; set; }
        [DataMember]
        public DateTime? DAPP { get; set; }
        [DataMember]
        public decimal? POURCENT { get; set; }
        [DataMember]
        public int? DELAI { get; set; }
        [DataMember]
        public decimal? MINIMUM { get; set; }
        [DataMember]
        public decimal? MAXIMUM { get; set; }
        [DataMember]
        public string COPER { get; set; }
        [DataMember]
        public string TRANS { get; set; }
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
