using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{


    [DataContract]
    public class CsReleveur
    {
        
       [DataMember] public string CENTRE { get; set; }
       [DataMember] public string CODE { get; set; }
       [DataMember] public string MATRICULE { get; set; }
       [DataMember] public Nullable<int> FERMEQUOT { get; set; }
       [DataMember] public Nullable<int> FERMEREAL { get; set; }
       [DataMember] public string PORTABLE { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public Nullable<bool> SUPPRIMER { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public Nullable<int> FK_IDUSER { get; set; }
       [DataMember] public string NOMRELEVEUR { get; set; }

        
    }

}
