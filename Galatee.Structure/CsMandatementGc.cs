using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsMandatementGc : CsPrint
    {
        [DataMember] public Nullable<decimal> MONTANT { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public string NUMEROMANDATEMENT { get; set; }
        [DataMember] public Nullable<int> FK_IDCAMPAGNA { get; set; }
        [DataMember] public string NOMGESTIONNAIRE { get; set; }
        [DataMember] public Nullable<decimal> MONTANTTOTALEMIS { get; set; }
        [DataMember] public Nullable<decimal> MONTANTTOTAL { get; set; }
        [DataMember] public Nullable<decimal> MONTANTTTC { get; set; }
        [DataMember] public Nullable<decimal> TAUXRECOUVREMENT { get; set; }
        [DataMember] public string PERIODE { get; set; }


        [DataMember] public string NUMEROCAMPAGNE { get; set; }   /* LKO 08/01/2021 */

        [DataMember] public virtual ICollection<CsDetailMandatementGc> DETAILMANDATEMENTGC_ { get; set; }
        [DataMember]  public virtual List<CsPaiementGc> PAIEMENTGC_ { get; set; }
    }
}
