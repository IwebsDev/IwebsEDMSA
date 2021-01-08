using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{

    [DataContract]
    public class CsCouleurActivite
    {
        [DataMember]  public int Couleur_ID { get; set; }
        [DataMember] public string Couleur_libelle { get; set; }
        [DataMember] public int Activite_ID { get; set; }
        [DataMember] public string Activite_Libelle { get; set; }
        [DataMember] public Guid  PK_ID { get; set; }
        

    }
}
