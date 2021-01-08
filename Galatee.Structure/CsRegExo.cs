using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsRegExo : CsPrint
    {
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string LIBELLECENTRE { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public string LIBELLEPRODUIT { get; set; }
        [DataMember]
        public string REGCLI { get; set; }
        [DataMember]
        public string LIBELLEREGCLI { get; set; }
        [DataMember]
        public string EXFAV { get; set; }
        [DataMember]
        public string EXFDOS { get; set; }
        [DataMember]
        public string EXFPOL { get; set; }
        [DataMember]
        public string TRAITFAC { get; set; }
        [DataMember]
        public string TRANS { get; set; }
        [DataMember]
        public int? REFERENCEPUPITRE { get; set; }
        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set;}
        [DataMember]
        public string USERMODIFICATION { get; set;}
        //[DataMember]
        //public string OriginalCENTRE { get; set; }
        //[DataMember]
        //public string OriginalPRODUIT { get; set; }
        //[DataMember]
        //public string OriginalREGCLI { get; set; }
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public int FK_IDCENTRE { get; set; }
        [DataMember]
        public int FK_IDPRODUIT { get; set; }
        [DataMember]
        public int FK_IDREGCLI { get; set; }
    }
}
