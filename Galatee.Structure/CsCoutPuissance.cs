using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsCoutpuissance : CsPrint
    {
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public decimal  PUISSANCE { get; set; }
        [DataMember]
        public string CODETARIF { get; set; }
        [DataMember]
        public string COPER { get; set; }
        [DataMember]
        public decimal?  MONTANT { get; set; }
        [DataMember]
        public string CALIBRE { get; set; }
        [DataMember]
        public bool  SUBVENTIONNEE { get; set; }

    }
 }









