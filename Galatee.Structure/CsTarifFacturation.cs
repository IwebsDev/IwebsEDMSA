using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTarifFacturation:CsPrint
    {
        [DataMember] public string PRODUIT { get; set; }
        [DataMember] public string REDEVANCE { get; set; }
        [DataMember] public string REGION { get; set; }
        [DataMember] public string SREGION { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string COMMUNE { get; set; }
        [DataMember] public string RECHERCHETARIF { get; set; }
        [DataMember] public string CTARCOMP { get; set; }
        [DataMember] public System.DateTime DEBUTAPPLICATION { get; set; }
        [DataMember] public Nullable<System.DateTime> FINAPPLICATION { get; set; }
        [DataMember] public string PERDEB { get; set; }
        [DataMember] public string PERFIN { get; set; }
        [DataMember] public string TAXE { get; set; }
        [DataMember] public string UNITE { get; set; }
        [DataMember] public Nullable<decimal> MONTANTANNUEL { get; set; }
        [DataMember] public Nullable<int> MINIVOL { get; set; }
        [DataMember] public Nullable<decimal> MINIVAL { get; set; }
        [DataMember] public Nullable<int> FORFVOL { get; set; }
        [DataMember] public Nullable<decimal> FORFVAL { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public int FK_IDCENTRE { get; set; }
        [DataMember] public int FK_IDVARIABLETARIF { get; set; }
        [DataMember] public int FK_IDTAXE { get; set; }
        [DataMember] public int FK_IDUNITECOMPTAGE { get; set; }
        [DataMember] public int PK_ID { get; set; }


        [DataMember] public string LIBELLEPRODUIT { get; set; }
        [DataMember] public string LIBELLEREDEVANCE { get; set; }
        [DataMember] public string LIBELLECENTRE { get; set; }
        [DataMember] public string LIBELLECOMMUNE { get; set; }
        [DataMember] public string LIBELLERECHERCHETARIF { get; set; }
        [DataMember] public string LIBELLETAXE { get; set; }
        [DataMember] public string MODEAPPLICATION { get; set; }
        [DataMember] public string MODECALCUL { get; set; }
        [DataMember] public string FORMULE { get; set; }
        [DataMember] public int FK_IDREDEVANCE { get; set; }
        [DataMember] public bool IsGenerationCtarcomp { get; set; }
        [DataMember] public List< CsDetailTarifFacturation> DETAILTARIFFACTURATION = new List<CsDetailTarifFacturation>();
    }
}
