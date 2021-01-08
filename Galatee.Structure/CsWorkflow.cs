using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsWorkflow
    {
        public System.Guid PK_ID { get; set; }
        public string WORKFLOWNAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string CODE { get; set; }
        public Nullable<int> FK_IDTABLE_TRAVAIL { get; set; }
        public string TABLENOM { get; set; }
        public string TABLENAME { get; set; }
        public string TABLEDESCRIPTION { get; set; }
    }
}
