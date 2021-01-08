using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract] 
    public class CsEditionDevis : CsPrint 
    {
        [DataMember] public string NUMDEMANDE { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string SITE { get; set; }
        [DataMember] public string NOM { get; set; }
        [DataMember] public string CODEPRODUIT { get; set; }
        [DataMember] public string TYPEDEMANDE { get; set; }
        [DataMember] public string PRODUIT { get; set; }
        [DataMember] public decimal TOTALDEVIS { get; set; }
        [DataMember] public string TELEPHONE { get; set; }
        [DataMember] public string COMMUNUE { get; set; }
        [DataMember] public string QUARTIER { get; set; }
        [DataMember] public string LONGITUDE { get; set; }
        [DataMember] public string LATITUDE { get; set; }

        [DataMember] public string DESIGNATION { get; set; }
        [DataMember] public decimal  QUANTITE { get; set; }
        [DataMember] public decimal  PRIXUNITAIRE { get; set; }
        [DataMember] public decimal  PRIXTVA { get; set; }
        [DataMember] public decimal  MONTANTHT { get; set; }
        [DataMember] public string   SECTION { get; set; }

        [DataMember] public string RUE { get; set; }
        [DataMember] public string PORTE { get; set; }
        [DataMember] public string DISTANCEBRT { get; set; }
        [DataMember] public string DISTANCEEXT { get; set; }
        [DataMember] public string PUISSANCE { get; set; }


    }
}
