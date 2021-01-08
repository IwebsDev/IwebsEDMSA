using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsCopieDmdCircuit
    {
        public System.Guid PK_ID { get; set; }
        public System.Guid FK_RWORKFLOWCENTRE { get; set; }
        public int FK_IDETAPE { get; set; }
        public int ORDRE { get; set; }
        public System.Guid FK_IDGROUPEVALIDATIOIN { get; set; }
        public string CONDITION { get; set; }
        public Nullable<bool> FROMCONDITION { get; set; }
        public Nullable<System.Guid> FK_IDRETAPEWORKFLOWORIGINE { get; set; }
        public Nullable<System.Guid> FK_IDDEMANDE { get; set; }
        public string CODE_DEMANDE { get; set; }
        public string CODEETAPE { get; set; }
        public string LIBELLEETAPE { get; set; }        
        public string GROUPEVALIDATION { get; set; }
        public string ETAPECONDITIONVRAIE { get; set; }
        public Nullable<bool> USEAFFECTATION { get; set; }

        public virtual CsGroupeValidation GROUPE_VALIDATION { get; set; }   
    }
}
