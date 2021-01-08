using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsLibruaction : CsPrint
    {
        [DataMember]
        public string PK_FK_CENTRE { get; set; }
        [DataMember]
        public string PK_FK_ACTION { get; set; }
        [DataMember]
        public string TRANS { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public string LNOMBRE1 { get; set; }
        [DataMember]
        public string LMONTANT1 { get; set; }
        [DataMember]
        public string LNOMBRE2 { get; set; }
        [DataMember]
        public string LMONTANT2 { get; set; }
        [DataMember]
        public string LNOMBRE3 { get; set; }
        [DataMember]
        public string LMONTANT3 { get; set; }
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
