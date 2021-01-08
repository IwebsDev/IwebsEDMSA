using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsLigneComptable
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public string COMPTE { get; set; }
        [DataMember]
        public string DC { get; set; }
        [DataMember]
        public Nullable<decimal> MONTANT { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public System.DateTime DATECREATION { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
    }
}
