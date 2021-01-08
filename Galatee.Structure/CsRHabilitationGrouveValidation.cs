using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsRHabilitationGrouveValidation
    {
        public System.Guid PK_ID { get; set; }
        public int RANG { get; set; }
        public int FK_IDADMUTILISATEUR { get; set; }
        public System.Guid FK_IDGROUPE_VALIDATION { get; set; }
        public System.DateTime DATE_HABILITATION { get; set; }
        public Nullable<System.DateTime> DATE_FIN_VALIDITE { get; set; }
        public string MATRICULE_USER_CREATION { get; set; }
        public Nullable<System.DateTime> DATE_DERNIEREMODIFICATION { get; set; }
        public string MATRICULE_USER_MODIFICATION { get; set; }
        public System.DateTime DATE_CREATION_HABILITATION { get; set; }
        public string LOGINNAME { get; set; }
        public string LIBELLE { get; set; }
        public string EMAIL { get; set; }
        public Nullable<bool> ESTRESPONSABLE { get; set; }
        public Nullable<bool> ESTCONSULTATION { get; set; }
    }
}
