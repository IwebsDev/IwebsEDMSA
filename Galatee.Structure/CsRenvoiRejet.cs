using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    //WCO le 14/01/2016
    //A une étape on a la possibilié de rejeter la demande à une étape donnée
    [DataContract]
    public class CsRenvoiRejet
    {
       [DataMember] public System.Guid PK_ID { get; set; }
       [DataMember] public System.Guid FK_IDRAFFECTATION { get; set; }
       [DataMember] public int FK_IDETAPE { get; set; }
       [DataMember] public int FK_IDETAPEACTUELLE { get; set; }
    } 
}
