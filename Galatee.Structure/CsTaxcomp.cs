using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTaxcomp : CsPrint
    {
        [DataMember]
        public string PK_FK_CENTRE { get; set; }
        [DataMember]
        public string PK_CTAX { get; set; }
        [DataMember]
        public string TAX1 { get; set; }
        [DataMember]
        public string CALCUL1 { get; set; }
        [DataMember]
        public string TAX2 { get; set; }
        [DataMember]
        public string CALCUL2 { get; set; }
        [DataMember]
        public string TAX3 { get; set; }
        [DataMember]
        public string CALCUL3 { get; set; }
        [DataMember]
        public string TAX4 { get; set; }
        [DataMember]
        public string CALCUL4 { get; set; }
        [DataMember]
        public string TAX5 { get; set; }
        [DataMember]
        public string CALCUL5 { get; set; }
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
    }
}
