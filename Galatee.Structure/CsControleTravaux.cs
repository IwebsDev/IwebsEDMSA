using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    [DataContract]
    public class CsControleTravaux
    {
       [DataMember] public string NUMDEM { get; set; }
       [DataMember] public int ORDRE { get; set; }
       [DataMember] public string MATRICULECHEFEQUIPE { get; set; }
       [DataMember] public string NOMCHEFEQUIPE { get; set; }
       [DataMember] public string METMOYCONTROLE { get; set; }
       [DataMember] public Nullable<System.DateTime> DATECONTROLE { get; set; }
       [DataMember] public string VOLUMETERTRVX { get; set; }
       [DataMember] public string DEGRADATIONVOIE { get; set; }
       [DataMember] public int NOTE { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int FK_IDMATRICULE { get; set; }
       [DataMember] public int FK_IDDEMANDE { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public string COMMENTAIRE { get; set; }
       [DataMember] public int FK_IDTYPECONTROLE { get; set; }
    }
}
