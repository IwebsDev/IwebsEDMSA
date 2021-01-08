using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsMapperDetailAffectationScelle
    {
        public System.Guid Id_DetailAffectationScelle { get; set; }
        public Nullable<System.Guid> Id_Affectation { get; set; }
        public string Nuemro_Scelle { get; set; }
        public System.DateTime Date_Reception { get; set; }
        public Nullable<int> Provenance_Id { get; set; }
        public string LotAppartenance_Id { get; set; }
        public Nullable<int> Centre_Origine_Scelle { get; set; }
        public Nullable<bool> EstLivre { get; set; }
        public string Commentaire { get; set; }
    }
}
