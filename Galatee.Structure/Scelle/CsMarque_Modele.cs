using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
 public   class CsMarque_Modele
    {
        [DataMember] public Guid PK_ID { get; set; }
        [DataMember] public int MODELE_ID { get; set; }
        [DataMember] public string Libelle_Modele { get; set; }
        [DataMember] public int MARQUE_ID { get; set; }
        [DataMember] public string Libelle_MArque { get; set; }
        [DataMember] public int Nbre_scel_capot { get; set; }
        [DataMember] public int Nbre_scel_cache { get; set; }
        [DataMember] public int Produit_ID { get; set; }
        [DataMember] public string Libelle_Produit { get; set; }
        [DataMember] public string CODE_Marque { get; set; }
    }
}
