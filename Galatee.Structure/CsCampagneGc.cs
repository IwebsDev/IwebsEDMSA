using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
   public class CsCampagneGc:CsPrint
    {
        [DataMember] public string NUMEROCAMPAGNE { get; set; }
        [DataMember] public string REGROUPEMENT { get; set; }
        [DataMember] public string LIBELLEREGROUPEMENT { get; set; }
        [DataMember] public Nullable<decimal> MONTANT { get; set; }
        [DataMember] public string MATRICULEGESTIONNAIRE { get; set; }
        [DataMember] public string MATRICULECREATEURCAMPAGNE { get; set; }
        [DataMember] public string PERIODE { get; set; }
        [DataMember] public int FK_IDREGROUPEMENT { get; set; }
        [DataMember] public int FK_IDMATRICULE { get; set; }
        [DataMember] public string STATUS { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember]  public string USERMODIFICATION { get; set; }
        [DataMember]  public string NOMAGENT { get; set; }

        //public virtual ADMUTILISATEUR ADMUTILISATEUR { get; set; }
        [DataMember]  public virtual CsRegCli REGCLI_ { get; set; }
        [DataMember]  public virtual List<CsDetailCampagneGc> DETAILCAMPAGNEGC_ { get; set; }
        [DataMember]  public virtual List<CsMandatementGc> MANDATEMENTGC_ { get; set; }
    }
}
