using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
    public class CsAnnotation
    {
        
       [DataMember] public string COMMENTAIRE { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public string MATRICULE { get; set; }
       [DataMember] public Nullable<int> FK_IDADMUTILISATEUR { get; set; }
       [DataMember] public Nullable<int> FK_IDDEMANDE { get; set; }
       [DataMember] public string LIBELLEAGENT { get; set; }

       [DataMember] public Nullable<int> FK_IDETAPE { get; set; }
       [DataMember] public string LIBELLEETAPE { get; set; }
    }
}
