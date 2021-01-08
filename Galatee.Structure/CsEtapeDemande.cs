using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsEtapeDemande
    {
        [DataMember] public string CONTROLEETAPE { get; set; }
       [DataMember] public string LIBELLEETAPE { get; set; }
       [DataMember] public string TDEM { get; set; }
       [DataMember] public int ORDRE { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public System.DateTime? DATECREATION { get; set; }
       [DataMember] public System.DateTime? DATEMODIFICATION { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDTDEM { get; set; }
    

    }

}









