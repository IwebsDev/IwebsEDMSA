using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCompteCritere : CsPrint
    {
        [DataMember]
        public string Centre { get; set; }
        [DataMember]
        public string Coper { get; set; }
        [DataMember]
        public string Produit { get; set; }
        [DataMember]
        public string catCli { get; set; }
        [DataMember]
        public string CompteGene { get; set; }
    }
}
