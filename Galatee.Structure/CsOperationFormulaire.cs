using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsOperationFormulaire
    {
        public System.Guid PK_ID { get; set; }
        public string CODE { get; set; }
        public string NOM { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<System.Guid> FK_ID_PARENTOPERATION { get; set; }
        public Nullable<int> FK_ID_PRODUIT { get; set; }
        public Nullable<int> FK_IDFORMULAIRE { get; set; }
        public string FORMULAIRE { get; set; }
        public string FULLNAMECONTROLE { get; set; }
        public bool CREATIONDEMANDE { get; set; }
    }
}
