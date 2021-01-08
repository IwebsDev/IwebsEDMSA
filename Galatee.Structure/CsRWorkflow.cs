using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsRWorkflow
    {
        public System.Guid PK_ID { get; set; }
        public System.Guid FK_IDWORKFLOW { get; set; }
        public System.Guid FK_IDOPERATION { get; set; }
        public int FK_IDCENTRE { get; set; }
    }
}
