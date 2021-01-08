using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsPaiementGc
    {
        [DataMember]
        public Nullable<decimal> MONTANT { get; set; }
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public System.DateTime DATECREATION { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
        [DataMember]
        public string NUMEROMANDATEMENT { get; set; }
        [DataMember]
        public string TYPE_PAIEMENT { get; set; }
        [DataMember]
        public bool EST_MIS_A_JOUR { get; set; }
        [DataMember]
        public string NumAvisCredit { get; set; }
        [DataMember]
        public Nullable<int> FK_IDMANDATEMANT { get; set; }
        [DataMember]
        public virtual ICollection<CsDetailPaiementGc> DETAILCAMPAGNEGC_ { get; set; }
    }
}
