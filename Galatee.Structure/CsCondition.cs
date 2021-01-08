using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsConditionBranchement
    {
        public System.Guid PK_ID { get; set; }
        public string NOM { get; set; }
        public string COLONNENAME { get; set; }
        public string VALUE { get; set; }
        public int FK_IDTABLETRAVAIL { get; set; }
        public Nullable<int> FK_IDETAPEVRAIE { get; set; }
        public Nullable<int> FK_IDETAPEFAUSE { get; set; }
        public string OPERATEUR { get; set; }
        public System.Guid FK_IDRAFFECTATIONWKF { get; set; }
        public bool PEUT_TRANSMETTRE_SI_FAUX { get; set; }
    }
}
