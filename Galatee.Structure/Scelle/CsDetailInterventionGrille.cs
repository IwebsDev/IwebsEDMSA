using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Scelle
{
    public class CsDetailInterventionGrille
    {
        public System.Guid ID_DetailInterventionDePoseDeScellesSurBranchementBTA_GrilleBTA { get; set; }
        public System.Guid ID_InterventionDePoseDeScellesSurBranchementBTA { get; set; }
        public System.Guid Grille_Bta_ID { get; set; }
        public System.Guid Ancien_Grille_Bta_ID { get; set; }
        public System.Guid Id_Scelle1 { get; set; }
        public System.Guid Id_Scelle2 { get; set; }
        public System.Guid Ancien_Id_Scelle1 { get; set; }
        public System.Guid Ancien_Id_Scelle2 { get; set; }
    }
}
