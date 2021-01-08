using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDomBanc : CsPrint
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public int FK_IDBANQUE { get; set; }
        [DataMember]
        public string BANQUE { get; set; }
        [DataMember]
        public string GUICHET { get; set; }
        [DataMember]
        public string TRANS { get; set; }
        [DataMember]
        public string COMPTE { get; set; }
        [DataMember]
        public string COMPTA { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
        [DataMember]
        public string LIBELLEBANQUE { get; set; }

        //public override string ToString()
        //{
        //    return this.LIBELLE;
        //}
    }
}
