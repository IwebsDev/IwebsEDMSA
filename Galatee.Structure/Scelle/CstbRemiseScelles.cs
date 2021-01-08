using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Scelle
{
   public  class CstbRemiseScelles
    {
        public System.Guid Id_Remise { get; set; }
        public System.DateTime Date_Remise { get; set; }
        public string Matricule_User { get; set; }
        public string Matricule_Receiver { get; set; }
        public int Motif_ID { get; set; }
        public Nullable<int> Nbre_Scelles { get; set; }
        public string CodeExploitation { get; set; }
        public Nullable<int> TypeRemise { get; set; }

        public  CsRefMotif RefMotif { get; set; }
        public  ICollection<CstbDetailRemiseScelles> tbDetailRemiseScelles { get; set; }
    }
}
