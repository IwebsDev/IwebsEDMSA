using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsVwConfigurationWorkflowCentre
    {
        public System.Guid PK_ID { get; set; }
        public string WORKFLOWNAME { get; set; }
        public System.Guid OPERATIONID { get; set; }
        public string NOM { get; set; }
        public string FORMULAIRE { get; set; }
        public Nullable<int> FK_IDFORMULAIRE { get; set; }
        public string CODECENTRE { get; set; }
        public string LIBELLE { get; set; }
        public int CENTREID { get; set; }
        public int FK_IDCODESITE { get; set; }
        public string CODESITE { get; set; }
    }
}
