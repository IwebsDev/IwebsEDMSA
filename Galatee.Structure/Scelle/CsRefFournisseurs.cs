using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
   public class CsRefFournisseurs
    {

        [DataMember]
        public int Fournisseur_ID { get; set; }
        [DataMember]
        public string Fournisseur_Libelle { get; set; }
    }
}
