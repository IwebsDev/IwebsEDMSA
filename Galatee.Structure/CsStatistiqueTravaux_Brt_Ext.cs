using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    [DataContract]
    public class CsStatistiqueTravaux_Brt_Ext:CsPrint
    {
        [DataMember]
        public string AGENCE { get; set; }
        [DataMember]
        public string LIBELLE_PRODUIT { get; set; }
        [DataMember]
        public int ID_PRODUIT { get; set; }
        [DataMember]
        public int Nbr2fils { get; set; }
        [DataMember]
        public decimal MONTANTHT2fils { get; set; }
        [DataMember]
        public decimal TVA2fils { get; set; }
        [DataMember]
        public decimal MONTANT_TOTAL2fils { get; set; }
        [DataMember]
        public int Nbr4fils { get; set; }
        [DataMember]
        public decimal MONTANTHT4fils { get; set; }
        [DataMember]
        public decimal TVA4fils { get; set; }
        [DataMember]
        public decimal MONTANT_TOTAL4fils { get; set; }
        [DataMember]
        public int Nbr_Extension { get; set; }
        [DataMember]
        public decimal MONTANTHT_Extension { get; set; }
        [DataMember]
        public decimal TVA_Extension { get; set; }
        [DataMember]
        public decimal MONTANT_TOTAL_Extension { get; set; }

    }
}
