using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsEtape
    {
        public int PK_ID { get; set; }
        public string CODE { get; set; }
        public string NOM { get; set; }
        public string CONTROLEETAPE { get; set; }
        public string DESCRIPTIONETAPE { get; set; }
        public Nullable<int> FK_IDMENU { get; set; }

        public Nullable<int> FK_IDFORMULAIRE { get; set; }
        public Nullable<bool> MODIFICATION { get; set; }
        public Nullable<System.Guid> FK_IDOPERATION { get; set; }
        public string LIBELLEFORMULAIRE { get; set; }
        public string FULLNAMECONTROLE { get; set; }
        public bool IS_TRAITEMENT_LOT { get; set; }
    }
}
