using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Scelle
{
    public class CstbDetailRemiseScelles
    {
        public System.Guid Id_DetailRemise { get; set; }
        public System.Guid Id_Remise { get; set; }
        public string Lot_Id { get; set; }
        public Nullable<System.Guid> Id_Scelle { get; set; }

        public CsScelle Scelles { get; set; }
        public CsTbLot  tbLot { get; set; }
        public CstbRemiseScelles tbRemiseScelles { get; set; }
    }
}
