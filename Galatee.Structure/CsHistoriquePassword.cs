using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsHistoriquePassword: CsPrint 
    {
        [DataMember] public string IDUSER { get; set; }
        [DataMember] public string PASSWORD { get; set; }
        [DataMember] public System.DateTime DATEENREGISTREMENT { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int FK_IDUSER { get; set; }
        [DataMember] public string NOMUSER { get; set; }
        [DataMember] public string POSTE { get; set; }

        
    }

}









