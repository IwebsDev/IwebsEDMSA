using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
        [DataContract]  
 public   class CsTrancheFraude
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int NUMTRANCHE { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public decimal PRIXUNITAIRE { get; set; }
        [DataMember] public Nullable<int> CONSOMAXI { get; set; }
        [DataMember] public int FK_IDREGLAGECOMPTEUR { get; set; }
        [DataMember] public int FK_IDPHASECOMPTEUR { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public int MontantHT { get; set; }
        [DataMember] public int MontantTva { get; set; }
        [DataMember]
        public int MontantTTC { get; set; }
        [DataMember]
        public int Quantite { get; set; }
    
    }
}
