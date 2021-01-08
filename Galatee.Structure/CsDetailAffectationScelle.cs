using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDetailAffectationScelle
    {
        [DataMember]
        public System.Guid Id_DetailAffectationScelle { get; set; }
        [DataMember]
        public Nullable<System.Guid> Id_Affectation { get; set; }
        [DataMember]
        public string Nuemro_Scelle { get; set; }
        [DataMember]
        public System.DateTime Date_Reception { get; set; }
        [DataMember]
        public Nullable<int> Provenance_Id { get; set; }
        [DataMember]
        public string LotAppartenance_Id { get; set; }
        [DataMember]
        public Nullable<int> Centre_Origine_Scelle { get; set; }
        [DataMember]
        public Nullable<bool> EstLivre { get; set; }
        [DataMember]
        public string Commentaire { get; set; }
    }
}
