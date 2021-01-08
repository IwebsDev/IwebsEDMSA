using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    public class CsEtapeDevis : CsPrint
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTypeDevis { get; set; }
        [DataMember]
        public string CodeProduit { get; set; }
        [DataMember]
        public int NumEtape { get; set; }
        [DataMember]
        public int IdTacheDevis { get; set; }
        [DataMember]
        public int IdTacheSuivante { get; set; }
        [DataMember]
        public int IdTacheIntermediaire { get; set; }
        [DataMember]
        public int IdTacheRejet { get; set; }
        [DataMember]
        public int IdTacheSaut { get; set; }
        [DataMember]
        public string CodeFonction { get; set; }
        [DataMember]
        public byte DelaiExecutionEtape { get; set; }
        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
    }
}
