using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure 
{
    [DataContract]
    public class CsQuartier : CsPrint
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public int FK_IDCOMMUNE { get; set; }
        [DataMember]
        public string CENTRE { get; set; }

        //[DataMember]
        //public string OriginalCENTRE { get; set; }

        [DataMember]
        public string COMMUNE { get; set; }

        //[DataMember]
        //public string OriginalCOMMUNE { get; set; }

        [DataMember]
        public string CODE { get; set; }

        //[DataMember]
        //public string OriginalQUARTIER { get; set; }

        [DataMember]
        public string LIBELLE { get; set; }

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

        [DataMember]
        public string LIBELLECENTRE { get; set; }

        [DataMember]
        public string LIBELLECOMMUNE { get; set; }

    }
}
