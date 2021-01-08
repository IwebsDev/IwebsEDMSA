using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsScelle
    {
        [DataMember]
        public Nullable<int> Status_ID { get; set; }
        [DataMember]
        public System.DateTime Date_creation_scelle { get; set; }
        [DataMember]
        public Nullable<int> Origine_scelle { get; set; }
        [DataMember]
        public Nullable<int> provenance_Scelle_ID { get; set; }
        [DataMember]
        public string Exploitation_Origine { get; set; }
        [DataMember]
        public string lot_ID { get; set; }
        [DataMember]
        public Nullable<int> Couleur_Scelle { get; set; }
        [DataMember]
        public string Exploitation_appartenance { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DateDePose { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DateDeRupture { get; set; }
        [DataMember]
        public Nullable<int> TypeOrganeScelle { get; set; }
        [DataMember]
        public string Reference_OrganeOuBranchementOuRaccordement_Scelle { get; set; }
        [DataMember]
        public System.Guid Id_Scelle { get; set; }
        [DataMember]
        public string Numero_Scelle { get; set; }
        [DataMember]
        public string NUM_SCELLE { get; set; }
        [DataMember]
        public int PK_ID { get; set; }
                [DataMember]
        public string Libelle_Couleur { get; set; }
        
    }
}
