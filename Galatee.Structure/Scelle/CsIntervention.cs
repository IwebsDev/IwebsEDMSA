using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Scelle
{
    public class CsIntervention
    {
        public System.Guid ID_InterventionDePoseDeScellesSurBranchementBTA { get; set; }
        public string Branchement_ID { get; set; }
        public System.Guid ID_FSB { get; set; }
        public string Matricule_Agent_Intervention { get; set; }
        public Nullable<System.DateTime> Date_Intervention { get; set; }
        public string Matricule_Agent_Saisie { get; set; }
        public Nullable<System.DateTime> Date_Saisie { get; set; }
        public Nullable<int> Motif_ID { get; set; }
    }
}
