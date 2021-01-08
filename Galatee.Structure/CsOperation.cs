using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsOperation
    {
        [DataMember] public System.Guid PK_ID { get; set; }
        [DataMember] public string CODE { get; set; }
        [DataMember] public string NOM { get; set; }
        [DataMember] public string DESCRIPTION { get; set; }
        [DataMember] public Nullable<System.Guid> FK_ID_PARENTOPERATION { get; set; }
        [DataMember] public Nullable<int> FK_ID_PRODUIT { get; set; }
        [DataMember]
        public string FORMULAIRE { get; set; }
        [DataMember]
        public Nullable<int> FK_IDFORMULAIRE { get; set; }
        [DataMember]
        public string PRODUITNAME { get; set; }
        [DataMember]
        public string CODE_TDEM { get; set; }
    }
}
