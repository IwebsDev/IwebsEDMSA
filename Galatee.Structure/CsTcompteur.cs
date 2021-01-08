using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTcompteur : CsPrint
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public int FK_IDCENTRE { get; set; }
        [DataMember] public string PRODUIT { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string CODE { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public string COMPTEUR { get; set; }
        [DataMember] public string SAISIE { get; set; }
        [DataMember] public int ? POINT { get; set; }
        [DataMember] public string TRANS { get; set; }
        [DataMember] public string MCOMPT { get; set; }
        [DataMember] public string CADCOMP { get; set; }
        [DataMember] public string TCOMP { get; set; }
        [DataMember] public string ANNEEFAB { get; set; }
        [DataMember] public DateTime? DATECREATION { get; set; }
        [DataMember] public DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
    }

}
