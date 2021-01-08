using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsGroupeValidation
    {
        public System.Guid PK_ID { get; set; }
        public string GROUPENAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string EMAILDIFFUSION { get; set; }
        public bool UNESEULEVALIDATION { get; set; }
        public int VALEURSPECIFIQUE { get; set; }
        public bool ESTCONSULTATION { get; set; }

        public Guid OPERATIONID { get; set; }
        public Guid WORKFLOWID { get; set; }
        public int IDETAPE { get; set; }
    }
}
