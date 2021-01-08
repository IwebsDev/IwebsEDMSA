using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsNotificaton
    {
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDTYPENOTIFICATION { get; set; }
       [DataMember] public string OBJET { get; set; }
       [DataMember] public string MESSAGE { get; set; }
       [DataMember] public string MENU { get; set; }

       [DataMember] public string LOGIN { get; set; }
       [DataMember] public string PASSWORD { get; set; }
       [DataMember] public string SERVEURSMTP { get; set; }
       [DataMember] public int PORT { get; set; }
       [DataMember] public Nullable<bool> SSL { get; set; }
    }
}
