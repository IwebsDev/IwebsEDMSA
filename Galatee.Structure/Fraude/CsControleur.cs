using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
   public class CsControleur : CsPrint
    {
         [DataMember]
         public int PK_ID { get; set; }
         [DataMember]
         public int FK_IDCONTROLE { get; set; }
         [DataMember]
         public int FK_IDUSERCONTROLEUR { get; set; }
         [DataMember]
         public bool IsChefEquipe { get; set; }
    }
}
