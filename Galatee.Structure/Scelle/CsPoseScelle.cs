using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Scelle
{
    public class CsPoseScelle
    {
        public CsIntervention Intervention { get; set; }
        public CsScelle LeScelle{ get; set; }
        public CsDetailInterventionCCP InterventionCCP { get; set; }
        public CsDetailInterventionCapotMoteur InterventionCapot { get; set; }
        public CsDetailInterventionCache InterventionCacheBorne { get; set; }
        public CsDetailInterventionDijoncteur InterventionDinjoncteur { get; set; }
        public CsDetailInterventionTableau InterventionTableau { get; set; }
        public CsDetailInterventionGrille InterventionGrille { get; set; }
        public CsBranchement LeBranchement { get; set; }
    }
}
