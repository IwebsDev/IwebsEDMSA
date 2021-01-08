using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsMapperAffectationScelle
    {
        public System.Guid Id_Affectation { get; set; }
        public Nullable<int> Code_Centre_Origine { get; set; }
        public Nullable<int> Code_Centre_Dest { get; set; }
        public System.DateTime Date_Transfert { get; set; }
        public Nullable<int> Id_Modificateur { get; set; }
        public Nullable<int> Nbre_Scelles { get; set; }
        public Nullable<int> Id_Demande { get; set; }
        public string NumScelleDepart { get; set; }
        public string NumScelleFin { get; set; }
    }
}
