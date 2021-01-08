using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsParamAbonUtilise : CsPrint
    {
        [DataMember]
        public string PK_ID { get; set; }
        [DataMember]
        public string FK_IDCENTRE { get; set; }
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string CLECAL { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public string PARAM { get; set; }
        [DataMember]
        public string CODE { get; set; }
        [DataMember]
        public string VALDEF { get; set; }
        [DataMember]
        public string STATUT { get; set; }
        [DataMember]
        public DateTime? DEBUTAPPLICATION { get; set; }
        [DataMember]
        public DateTime? FINAPPLICATION { get; set; }
        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
        //[DataMember]
        //public string OriginalCENTRE { get; set; }
        //[DataMember]
        //public string OriginalCLECAL { get; set; }
        //[DataMember]
        //public string OriginalPRODUIT { get; set; }
        //[DataMember]
        //public string OriginalPARAM { get; set; }
    }
}
