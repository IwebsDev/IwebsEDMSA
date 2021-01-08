using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDetailMandatementGc : CsPrint
    {
        [DataMember]
        public int FK_IDMANDATEMENT { get; set; }
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string CLIENT { get; set; }
        [DataMember]
        public string ORDRE { get; set; }
        [DataMember]
        public string PERIODE { get; set; }
        [DataMember]
        public string NDOC { get; set; }
        [DataMember]
        public string STATUS { get; set; }
        [DataMember]
        public Nullable<decimal> MONTANT { get; set; }
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public int FK_IDCLIENT { get; set; }
        [DataMember]
        public int FK_IDLCLIENT { get; set; }
        [DataMember]
        public System.DateTime DATECREATION { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
        [DataMember]
        public Nullable<decimal> MONTANT_REGLER { get; set; }
        [DataMember]
        public Nullable<decimal> MONTANT_RESTANT { get; set; }
        [DataMember]
        public Nullable<decimal> MONTANT_VERSER { get; set; }
        [DataMember]
        public bool IsMontantValide { get; set; }
         [DataMember]
        public Nullable<decimal> MONTANTTVA { get; set; }
         [DataMember]
         public Nullable<decimal> MONTANTHT { get; set; }
        
    }
}
