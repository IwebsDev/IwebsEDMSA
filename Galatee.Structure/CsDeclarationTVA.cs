using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDeclarationTVA : CsPrint
    {
        [DataMember]
        public string Categorie { get; set; }
        [DataMember]
        public decimal MontantFacture { get; set; }
        [DataMember]
        public decimal MontantTVA { get; set; }
        [DataMember]
        public decimal MontantConso { get; set; }
        [DataMember]
        public decimal MontantLocation { get; set; }
        [DataMember]
        public decimal MontantEntretien { get; set; }
        [DataMember]
        public decimal MontantTutelle { get; set; }
        [DataMember]
        public string Mois { get; set; }
        [DataMember]
        public string Annee { get; set; }
    }
}
