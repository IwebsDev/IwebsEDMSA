using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsRedevance : CsPrint
    {
        [DataMember] public List<CsTrancheRedevence> TRANCHEREDEVANCE = new List<CsTrancheRedevence>();
        [DataMember] public int FK_IDTYPEREDEVANCE { get; set; }
        [DataMember] public int FK_IDTYPELIENREDEVANCE { get; set; }
        [DataMember] public string CODE { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string PRODUIT { get; set; }
  
        [DataMember] public string NATURECLI { get; set; } // CRITERE
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public string GRATUIT { get; set; }
        [DataMember] public string EXONERATION { get; set; }
        [DataMember] public string TYPELIEN  { get; set; }
        [DataMember] public string TRANCHE { get; set; }
        [DataMember] public string PARAM1 { get; set; }
        [DataMember] public string PARAM2 { get; set; }
        [DataMember] public string PARAM3 { get; set; }
        [DataMember] public string PARAM4 { get; set; }
        [DataMember] public string PARAM5 { get; set; }
        [DataMember] public string PARAM6 { get; set; }
        [DataMember] public string NUMREDEVANCE { get; set; }
        [DataMember] public string EDITEE { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public DateTime? DATECREATION { get; set; }
        [DataMember] public DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public string LIBELLECENTRE { get; set; }
        [DataMember] public string LIBELLEPRODUIT { get; set; }
        [DataMember] public string LIBELLENATURECLIENT { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public int FK_IDCENTRE { get; set; }

        [DataMember] public decimal  MONTANT { get; set; }


    }
}
