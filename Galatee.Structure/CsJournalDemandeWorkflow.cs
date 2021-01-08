using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsJournalDemandeWorkflow
    {
        public System.Guid PK_ID { get; set; }
        public System.DateTime DATEACTION { get; set; }
        public string MATRICULEUSERACTION { get; set; }
        public string LIBELLEACTION { get; set; }
        public string OBSERVATIONS { get; set; }
        public System.Guid FK_IDDEMANDE { get; set; }
        public string CODE_DEMANDE { get; set; }
    }
}
