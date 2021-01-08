using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Scelle
{
    public class CsDetailInterventionCapotMoteur
    {
        public System.Guid ID_DetailInterventionDePoseDeScellesSurBranchementBTA_CompteurBTA { get; set; }
        public System.Guid ID_InterventionDePoseDeScellesSurBranchementBTA { get; set; }
        public string CompteurBTA_Numero { get; set; }
        public string CompteurBTA_AncienNumero { get; set; }
        public System.Guid CapotMoteur_Id_Scelle1 { get; set; }
        public System.Guid CapotMoteur_Id_Scelle2 { get; set; }
        public System.Guid CapotMoteur_Id_Scelle3 { get; set; }
    }
}
