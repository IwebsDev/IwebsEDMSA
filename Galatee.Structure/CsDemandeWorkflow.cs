using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsDemandeWorkflow
    {
        public System.Guid PK_ID { get; set; }
        public string CODE { get; set; }
        public int FK_IDCENTRE { get; set; }
        public System.Guid FK_IDOPERATION { get; set; }
        public System.Guid FK_IDWORKFLOW { get; set; }
        public System.Guid FK_IDRWORKLOW { get; set; }
        public System.DateTime DATECREATION { get; set; }
        public int FK_IDTABLETRAVAIL { get; set; }
        public bool ALLCENTRE { get; set; }
        public int FK_IDETAPEACTUELLE { get; set; }
        public int FK_IDETAPEPRECEDENTE { get; set; }
        public int FK_IDETAPESUIVANTE { get; set; }
        public string MATRICULEUSERCREATION { get; set; }
        public Nullable<int> FK_IDSTATUS { get; set; }
        public string FK_IDLIGNETABLETRAVAIL { get; set; }
        public string MATRICULEUSERMODIFICATION { get; set; }
        public Nullable<System.DateTime> DATEDERNIEREMODIFICATION { get; set; }
        public Nullable<System.Guid> FK_IDETAPECIRCUIT { get; set; }
        public string CODE_DEMANDE_TABLETRAVAIL { get; set; }
    }
}
