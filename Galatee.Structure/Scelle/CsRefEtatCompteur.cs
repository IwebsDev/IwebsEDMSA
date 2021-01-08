using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Inova.Tools.Utilities;
namespace Galatee.Structure
{
     [DataContract]
    public class CsRefEtatCompteur : CsPrint
    {
        [DataMember]
         public int EtatCompteur_ID { get; set; }
        [DataMember]
        public string Libelle_ETAT { get; set; }
       
    }
}
