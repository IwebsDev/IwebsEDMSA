using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDetailCampagnePrecontentieux : CsPrint
    {
        
       [DataMember] public string IDCAMPAGNE { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public string CENTRE { get; set; }
       [DataMember] public string CLIENT { get; set; }
       [DataMember] public string ORDRE { get; set; }
       [DataMember] public string NOMABON { get; set; }
       [DataMember] public string ADRESSE { get; set; }
       [DataMember] public string RUE { get; set; }
       [DataMember] public string PORTE { get; set; }
       [DataMember] public string TOURNEE { get; set; }
       [DataMember] public string ORDTOUR { get; set; }
       [DataMember] public string CATEGORIE { get; set; }
       [DataMember] public Nullable<decimal> SOLDEDUE { get; set; }
       [DataMember] public Nullable<decimal> SOLDECLIENT { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public int FK_IDCLIENT { get; set; }
       [DataMember] public int FK_IDTOURNEE { get; set; }
       [DataMember] public int FK_IDCATEGORIE { get; set; }
       [DataMember] public int FK_IDCAMPAGNE { get; set; }
       [DataMember] public bool ISINVITATIONEDITER { get; set; }
       [DataMember] public DateTime?  DATERESILIATION { get; set; }
       [DataMember] public DateTime?  DATERDV { get; set; }

       // Autre
       [DataMember] public bool IsSelect { get; set; }
       [DataMember] public string  LIBELLECENTRE { get; set; }
       [DataMember] public string  STATUSINVITATION { get; set; }

        
        
    }
}
