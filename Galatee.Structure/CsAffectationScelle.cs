using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsAffectationScelle
    {
        [DataMember]
        public System.Guid Id_Affectation { get; set; }
        [DataMember]
        public Nullable<int> Code_Centre_Origine { get; set; }
        [DataMember]
        public Nullable<int> Code_Centre_Dest { get; set; }
        [DataMember]
        public System.DateTime Date_Transfert { get; set; }
        [DataMember]
        public Nullable<int> Id_Modificateur { get; set; }
        [DataMember]
        public Nullable<int> Nbre_Scelles { get; set; }
        [DataMember]
        public Nullable<int> Id_Demande { get; set; }
    }
}
