using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsInfoDemandeWorkflow
    {
        [DataMember] public Guid PK_ID { get; set; }
        [DataMember] public String CODE { get; set; }
        [DataMember] public string CODE_DEMANDE_TABLE_TRAVAIL { get; set; }
        [DataMember] public string IDLIGNETABLETRAVAIL { get; set; }
        [DataMember] public Guid FK_IDOPERATION { get; set; }
        [DataMember] public string CODETDEM { get; set; }
        [DataMember] public string ETAPE_ACTUELLE { get; set; }
        [DataMember] public string ETAPE_PRECEDENTE { get; set; }
        [DataMember] public string ETAPE_SUIVANTE { get; set; }
        [DataMember] public int FK_IDCENTRE { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public int FK_IDSITE { get; set; }
        [DataMember] public string SITE { get; set; }
        [DataMember] public string CODEETAPE { get; set; }

        [DataMember] public int FK_IDETAPEACTUELLE { get; set; }
        [DataMember] public int FK_IDETAPESUIVANTE { get; set; }
        [DataMember] public Guid FK_IDWORKFLOW{ get; set; }
        [DataMember] public string LIBELLEDEMANDE { get; set; }

        [DataMember] public DateTime DATECREATION { get; set; }
        [DataMember] public List<CsRenvoiRejet> LiteRejet { get; set; }
        [DataMember] public List<CsUtilisateur> UtilisateurEtapeSuivante { get; set; }
        [DataMember] public int FK_IDSTATUS { get; set; }
    }
}
